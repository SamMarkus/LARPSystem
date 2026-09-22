using GitAmendStateMachine;
using UnityEngine;

public class GitCollectPickupState : BaseState
{
    GameObject pickup;

    public GitCollectPickupState(AIAgent agent) : base(agent) {}
    public override void FixedUpdate() { }
    public override void OnEnter() { pickup = null; }
    public override void OnExit() { }
    public override void Update() 
    {
        if (!pickup)
        {
            pickup = FindPickup(agent);

            if (pickup)
            {
                CollectPickup(agent, pickup);
            }
        }
    }

    GameObject FindPickup(AIAgent agent)
    {
        if (agent.sensor.Objects.Count > 0)
        {
            return agent.sensor.Objects[0];
        }
        return null;
    }

    void CollectPickup(AIAgent agent, GameObject pickup)
    {
        agent.navMeshAgent.destination = pickup.transform.position;
    }
}

