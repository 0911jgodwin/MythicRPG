using System.Collections.Generic;

public class Sequence : Node
{
    public Sequence() { }

    public override NodeStates Evaluate()
    {
        NodeStates childStatus = children[currentChild].Evaluate();
        if (childStatus == NodeStates.RUNNING) return NodeStates.RUNNING;
        if (childStatus == NodeStates.FAILURE) return childStatus;

        currentChild++;
        if(currentChild >= children.Count)
        {
            currentChild = 0;
            return NodeStates.SUCCESS;
        }
        return NodeStates.RUNNING;
    }


}
