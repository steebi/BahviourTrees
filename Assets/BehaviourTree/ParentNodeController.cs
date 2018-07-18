using System.Collections.Generic;

namespace BehaviourTrees
{
    public class ParentNodeController: TaskController
    {
        private List<Node> m_subnodes;
        public IList<Node> SubNodes { get { return m_subnodes.AsReadOnly(); } }

        public Node CurrentNode { get; set; }

        public ParentNodeController(Node node): base(node)
        {
            m_subnodes = new List<Node>();
            CurrentNode = null;
        }

        public void Add(Node node)
        {
            m_subnodes.Add(node);
        }

        public new void Reset()
        {
            base.Reset();
            CurrentNode = m_subnodes.Count > 0 ? m_subnodes[0]:null;
        }
    }
}