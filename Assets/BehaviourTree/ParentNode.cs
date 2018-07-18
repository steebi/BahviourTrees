using UnityEngine;

namespace BehaviourTrees
{
    public abstract class ParentNode : Node
    {
        new public ParentNodeController Controller { get; set; }

        public ParentNode(BlackBoard bb) : base(bb)
        {
            Controller = new ParentNodeController(this);
        }

        public override bool CheckCondition()
        {
            return Controller.SubNodes.Count > 0;
        }

        public override void DoAction()
        {
            if (Controller.Completed)
                return;
            if (Controller.CurrentNode == null)
                return;
            if (Controller.CurrentNode.Controller.Started)
                Controller.CurrentNode.Controller.SafeStart();
            else if (Controller.CurrentNode.Controller.Completed)
            {
                Controller.CurrentNode.Controller.SafeEnd();
                if (Controller.CurrentNode.Controller.Success)
                    ChildSuceeded();
                else
                    ChildFailed();
            }
            else
                Controller.CurrentNode.DoAction();
        }

        public override void End()
        {
            Debug.Log("Ending!");
        }

        public override void Start()
        {
            Debug.Log("Starting");
            if (Controller.SubNodes.Count > 0)
                Controller.CurrentNode = Controller.SubNodes[0];
            else
            {
                Controller.CurrentNode = null;
                Debug.Log("No starting node was found!");
            }
        }

        public abstract void ChildSuceeded();
        public abstract void ChildFailed();

    }
}
