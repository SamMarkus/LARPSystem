using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIChasePlayerState : AIState
{
    public Transform playerTransform;
    float timer = 0.0f; 

    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }


    public void Enter(AIAgent agent)
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return; 
        }

        timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = playerTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance* agent.config.maxDistance)
            {
                if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = playerTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}
