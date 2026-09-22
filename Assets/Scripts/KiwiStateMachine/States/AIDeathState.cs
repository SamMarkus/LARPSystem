using Unity.AppUI.UI;
using UnityEngine;

public class AIDeathState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.Death; 
    }

    public void Enter(AIAgent agent)
    { 
        // Enter death functions here
    }

    public void Update(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {
        
    }
}
