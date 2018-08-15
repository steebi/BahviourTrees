namespace BehaviourTree
{
    public class SequencerNode : IBaseNode
    {
        private IBaseNode[] m_nodes;
        private int m_currentNode = 0;

        public SequencerNode(params IBaseNode[] inputNodes)
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
            return ExecuteNodes(m_currentNode);
        }

        private Status ExecuteNodes(int startIndex)
        {
            for (int i = startIndex; i < m_nodes.Length; ++i)
            {
                Status stat = m_nodes[i].OnTick();
                switch (stat)
                {
                    case Status.Success:
                        continue;
                    case Status.Running:
                        m_currentNode = i;
                        return Status.Running;
                    case Status.Failure:
                        return Status.Failure;
                }
            }
            return Status.Success;
        }
    }
}