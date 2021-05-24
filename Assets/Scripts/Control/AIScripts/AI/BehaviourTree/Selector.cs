using System.Collections.Generic;

public class Selector : Node
{
    public Selector() {    }

    public override NodeStates Evaluate()
    {
        NodeStates childStatus = children[currentChild].Evaluate();
        if (childStatus == NodeStates.RUNNING) return NodeStates.RUNNING;

        if (childStatus == NodeStates.SUCCESS)
        {
            currentChild = 0;
            return NodeStates.SUCCESS;
        }

        currentChild++;
        if(currentChild >= children.Count)
        {
            currentChild = 0;
            return NodeStates.FAILURE;
        }

        return NodeStates.RUNNING;
    }
}
