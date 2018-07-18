namespace BehaviourTrees
{
    public class Selector : ParentNode
    {
        public Selector(BlackBoard bb) : base(bb) { }

        public Node ChooseNewNode()
        {
            Node node = null;
            bool found = false;
            int position = Controller.SubNodes.IndexOf(Controller.CurrentNode);

            while (!found)
            {
                if (position == Controller.SubNodes.Count - 1)
                {
                    found = true;
                    node = null;
                    break;
                }
                position++;
                node = Controller.SubNodes[position];
                if (node.CheckCondition())
                    found = true;
            }
            return node;
        }

        public override void ChildFailed()
        {
            Controller.CurrentNode = ChooseNewNode();
            if (Controller.CurrentNode == null)
                Controller.FinishWithFailure();
        }

        public override void ChildSuceeded()
        {
            Controller.FinishWithSuccess();
        }
    }
}