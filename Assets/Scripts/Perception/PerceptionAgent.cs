using System.Collections.Generic;

using UnityEngine; 


namespace Perception
{
    public class PerceptionAgent : MonoBehaviour
    {
        #region Fields

        [Header("Sight")]
        [SerializeField] float viewRange = 15;
        [SerializeField] float primaryFov = 90f;
        [SerializeField] float periphrealFov = 160f;
        [SerializeField] LayerMask obstructionMask = ~0;

        [Header("Awareness")]
        [SerializeField] float alertThreshold = 25f;
        [SerializeField] float highAlertEnter = 50f;
        [SerializeField] float highAlertExit = 35f;
        [SerializeField] float turnSpeed = 4f;

        readonly Dictionary<Transform, TargetTrack> tracks = new();
        readonly Dictionary<Transform, HashSet<StimType>> active = new();
        static readonly StimType[] allTypes = { StimType.VisualPrimary, StimType.VisualPeripheral, StimType.AudioMovement, StimType.AudioLoud };
        bool latchedHighAlert;
        Renderer rend;
        Material mat;
        static readonly Color idle = new Color(0.2f, 0.8f, 0.3f),
            suspicious = new Color(1f, 0.85f, 0.1f),
            alert = new(1f, 0.2f, 0.15f);


        #endregion

        private void LateUpdate()
        {
            ScanSight();
            TickTracks();
            React();
            active.Clear(); 
        }

        void React()
        {
            TargetTrack best = null;
            Transform bestTarget = null; 

            foreach (var pair in tracks)
            {
                if (best == null || pair.Value.Score > best.Score)
                {
                    best = pair.Value;
                    bestTarget = pair.Key;
                }
            }

            var score = best != null ? best.Score : 0f; 
            var isPerceiving = bestTarget && active.TryGetValue(bestTarget, out var set) && set.Count > 0;

            if (best != null && score >= alertThreshold && isPerceiving)
            {
                var dir = best.LastKnownPosition - transform.position;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.01f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
            }

            if (score >= highAlertEnter) latchedHighAlert = true;
            else if (score < highAlertExit || score <= 0f) latchedHighAlert = false;

            if (mat) mat.color = latchedHighAlert ? alert : score >= alertThreshold ? suspicious : idle;
        }

        void TickTracks()
        {
            var remove = new List<Transform>();
            
            foreach (var pair in tracks)
            {
                foreach (var t in allTypes)
                    pair.Value.Tick(t, active.TryGetValue(pair.Key, out var set) && set.Contains(t), Time.deltaTime);

                if (pair.Value.Score <= 0f) remove.Add(pair.Key); 
            }

            foreach (var t in remove) tracks.Remove(t);
        }

        void ScanSight()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;

            var target = player.transform;
            var eye = transform.position + Vector3.up;
            var to = target.position + Vector3.up - eye;
            var dist = to.magnitude;
            if (dist > viewRange) return;

            var angle = Vector3.Angle(transform.forward, to);
            StimType? type = null;
            if (angle <= primaryFov * 0.5f) type = StimType.VisualPrimary;
            else if (angle <= periphrealFov * 0.5f) type = StimType.VisualPeripheral;

            if (type == null || Physics.Raycast(eye, to.normalized, dist, obstructionMask)) return;

            PerceptionHub.Emit(new Stim(type.Value, target, target.position, viewRange), transform); 
        }

        public void Recieve(Stim stim, Transform observer = null)
        {
            if (stim.Type == StimType.VisualPrimary || stim.Type == StimType.VisualPeripheral)
            {
                if (observer != transform) return; 
            }

            if (!stim.Source || Vector3.Distance(transform.position, stim.Position) > RangeFor(stim)) return;

            if (!tracks.TryGetValue(stim.Source, out var track))
            {
                track = new TargetTrack { Target = stim.Source };
                tracks[stim.Source] = track;
            }

            var config = ConfigFor(stim.Type);
            track.Feed(stim.Type, config.peak, config.attack, config.release, stim.Position);

            if (!active.TryGetValue(stim.Source, out var set))
            {
                set = new HashSet<StimType>();
                active[stim.Source] = set; 
            }

            set.Add(stim.Type);
        }

        float RangeFor(Stim s) => s.Type == StimType.AudioMovement || s.Type == StimType.AudioLoud ? s.Radius : viewRange; 

        (float peak, float attack, float release) ConfigFor(StimType t)
        {
            return t switch
            {
                StimType.VisualPrimary => (100f, 2f, 40f),
                StimType.VisualPeripheral => (40f, 4f, 40f),
                StimType.AudioMovement => (25f, 1f, 8f),
                _ => (80f, 0.5f, 18f)
            };
        }

        void Awake()
        {
            rend = GetComponent<Renderer>();
            if (rend) mat = rend.material; 
        }

        void OnEnable() => PerceptionHub.Register(this); 
        void OnDisable () => PerceptionHub.Unregister(this);
    }
}