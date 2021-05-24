using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate NodeStates Tick();
    public Tick EvaluateMethod;

    public Leaf() { }

    public Leaf(Tick method)
    {
        EvaluateMethod = method;
    }

    public override NodeStates Evaluate()
    {
        if (EvaluateMethod != null)
        {
            return EvaluateMethod();
        }
        return NodeStates.FAILURE;
    }
}
