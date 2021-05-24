using System.Collections.Generic;

[System.Serializable]
public abstract class Node
{
    protected NodeStates nodeState;
    public NodeStates GetNodeState
    {
        get { return nodeState; }
    }

    public List<Node> children = new List<Node>();
    public int currentChild = 0;

    public Node() { }

    public void AddChild(Node n)
    {
        children.Add(n);
    }

    public virtual NodeStates Evaluate()
    {
        return children[currentChild].Evaluate();
    }

}