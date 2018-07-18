namespace BehaviourTrees
{
    public abstract class LeafNode: Node
    {

        public TaskController Contoller { get; set; }

        public LeafNode(BlackBoard bb): base(bb)
        {
            Contoller = new TaskController(this);
        }
    }
}