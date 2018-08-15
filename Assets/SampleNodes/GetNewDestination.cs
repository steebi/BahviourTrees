using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class GetNewDestination: IBaseNode
{
    private NavMeshAgent m_agent;
    private float m_targetRadius;

    public GetNewDestination(NavMeshAgent agent, float targetRadius = 10.0f)
    {
        m_agent = agent;
        m_targetRadius = targetRadius;
    }

    public void OnInitialize()
    {

    }

    public void OnTermination()
    {

    }

    public Status OnTick()
    {
        Vector3 targetDirection = Random.insideUnitSphere * Random.Range(1.0f, m_targetRadius);
        NavMeshHit targetPoint;
        if (NavMesh.SamplePosition(targetDirection, out targetPoint, 1.0f, 1))
        {
            m_agent.destination = targetPoint.position;
            return Status.Success;
        }
        return Status.Failure;
    }
}
