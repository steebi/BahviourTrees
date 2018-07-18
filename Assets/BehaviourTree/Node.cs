namespace BehaviourTrees
{
    public abstract class Node
    {
        public TaskController m_controller;
        public TaskController Controller { get { return m_controller; } }
        public BlackBoard BlackBoard { get; set; }

        public abstract bool CheckCondition();
        public abstract void Start();
        public abstract void End();
        public abstract void DoAction();

        public Node(BlackBoard bb)
        {
            BlackBoard = bb;
        }
    }
}
