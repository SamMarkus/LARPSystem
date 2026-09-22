using UnityEngine;

public enum AIStateID
{
    ChasePlayer,
    Idle,
    Death,
    FindPickup,
    Roam
}

public interface AIState 
{
    AIStateID GetID();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent); 
}
