using GitAmendStateMachine;
using UnityEngine;
using UnityEngine.AI;

public class GitRoamState : BaseState
{
    public Transform centrePoint;
    public NavMeshAgent navAgent;
    public AIAgentConfig agentConfig;

    public GitRoamState(AIAgent agent) : base(agent) { }
    public override void OnEnter() 
    {
        centrePoint = agent.transform;
        navAgent = agent.navMeshAgent;
        agentConfig = agent.config;
    }
    public override void OnExit() { }
    public override void Update() 
    {
        if (navAgent.remainingDistance <= navAgent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, agentConfig.roamRange, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                navAgent.SetDestination(point);
            }
        }
    }

    public override void FixedUpdate() { }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
