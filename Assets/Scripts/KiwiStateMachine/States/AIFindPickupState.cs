using UnityEngine;

public class AIFindPickupState : AIState
{
    GameObject pickup;

    public AIStateID GetID()
    {
        return AIStateID.FindPickup;
    }
    public void Enter(AIAgent agent)
    {
        pickup = null;
    }

    public void Update(AIAgent agent)
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

    public void Exit(AIAgent agent)
    {

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
