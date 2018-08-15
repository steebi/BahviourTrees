using UnityEngine;
using BehaviourTree;

public class AlwaysSuceed : IBaseNode
{
    private IBaseNode m_childNodes = null;

    public AlwaysSuceed()
    {

    }

    public AlwaysSuceed(IBaseNode childNodes)
    {
        m_childNodes = childNodes;
    }

    public void OnInitialize()
    {

    }

    public void OnTermination()
    {

    }

    public Status OnTick()
    {
        if (m_childNodes != null)
            m_childNodes.OnTick();
        return Status.Success;
    }
}