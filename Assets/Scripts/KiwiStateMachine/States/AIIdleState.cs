using UnityEngine;

public class AIIdleState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {
      
    }
    public void Update(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
        
    }
}
