﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public void Enter(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateID GetID()
    {
        return AIStateID.DEATH;
    }

    public void Update(AIAgent agent)
    {
        throw new System.NotImplementedException();
    }
}
