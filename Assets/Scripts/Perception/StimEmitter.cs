using UnityEngine;

namespace Perception
{
    public class StimEmitter : MonoBehaviour
    {
        //[SerializeField] float walkSpeed = 1.5f;
        //[SerializeField] float sprintSpeed = 4f;
        //[SerializeField] float walkRadius = 8f;
        //[SerializeField] float sprintRadius = 18f;

        CharacterController player;
        Transform body; 


        void Awake()
        {
            player = GetComponent<CharacterController>();
            body = transform; 
        }

        private void Update()
        {
            if (!player || !body) return;
            /*
            var vel = player.GetMovementVelocity(); 
            var speed = vel.magnitude;
            if (speed < walkSpeed) return; 
            var loud = speed >= sprintSpeed;
             
            PerceptionHub.Emit(
                new Stim(
                    loud ? StimType.AudioLoud : StimType.AudioMovement,
                    body,
                    body.position,
                    loud ? sprintRadius : walkRadius)
            
                );
             */
        }
    }
}
