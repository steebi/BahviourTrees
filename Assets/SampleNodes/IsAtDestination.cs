using BehaviourTree;
using UnityEngine.AI;
using UnityEngine;

public class IsAtDestination : IBaseNode
{
    private NavMeshAgent m_agent;

    public IsAtDestination(NavMeshAgent agent)
    {
        m_agent = agent;
    }

    public void OnInitialize()
    {

    }

    public void OnTermination()
    {

    }

    public Status OnTick()
    {
        if (m_agent.velocity == Vector3.zero && !m_agent.hasPath)
            return Status.Success;
        else
            return Status.Failure;
    }
}
