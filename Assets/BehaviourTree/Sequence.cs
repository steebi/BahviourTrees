namespace BehaviourTrees
{
    public class Sequence: ParentNode
    {
        public Sequence(BlackBoard bb) : base(bb) { }

        public override void ChildSuceeded()
        {
            int position = Controller.SubNodes.IndexOf(Controller.CurrentNode);
            if (position == Controller.SubNodes.Count - 1)
                Controller.FinishWithSuccess();
            else
            {
                Controller.CurrentNode = Controller.SubNodes[position + 1];
                if (!Controller.CurrentNode.CheckCondition())
                    Controller.FinishWithFailure();
            }
        }

        public override void ChildFailed()
        {
            Controller.FinishWithFailure();
        }
    }
}