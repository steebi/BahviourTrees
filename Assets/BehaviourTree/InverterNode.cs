using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class InverterNode : IBaseNode
    {
        private IBaseNode m_subNode;

        public InverterNode(IBaseNode subNode)
        {
            m_subNode = subNode;
        }

        public void OnInitialize()
        {

        }

        public void OnTermination()
        {

        }

        public Status OnTick()
        {
            Status status = m_subNode.OnTick();
            switch (status)
            {
                case Status.Success:
                    return Status.Failure;
                case Status.Failure:
                    return Status.Success;
                default:
                    return Status.Running;
            }
        }
    }
}