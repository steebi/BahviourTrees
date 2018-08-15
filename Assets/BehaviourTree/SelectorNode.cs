namespace BehaviourTree
{
    public class SelectorNode : IBaseNode
    {
        private IBaseNode[] m_nodes;
        private int m_currentNode = 0;

        public SelectorNode(params IBaseNode[] inputNodes)
        {
            m_nodes = inputNodes;
        }

        public void OnInitialize()
        {

        }

        public void OnTermination()
        {

        }

        public Status OnTick()
        {
            return FindSuccess(m_currentNode);
        }

        private Status FindSuccess(int startIndex)
        {
            for (int i = startIndex; i < m_nodes.Length; ++i)
            {
                Status stat = m_nodes[i].OnTick();
                switch (stat)
                {
                    case Status.Success:
                        return Status.Success;
                    case Status.Running:
                        m_currentNode = i;
                        return Status.Running;
                    case Status.Failure:
                        continue;
                }
            }
            return Status.Failure;
        }
    }
}