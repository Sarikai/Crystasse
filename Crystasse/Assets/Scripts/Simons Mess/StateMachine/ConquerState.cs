﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquerState : State
{
    public UnitAccess Agent { get; private set; }
    public Crystal Target;


    protected override void Enter()
    {
        Substate = Substates.Stay;
    }

    protected override void Exit()
    {
        Completed = true;
    }

    protected override void Stay()
    {
        if(Target.TeamID != Agent.TeamID)
            Target.Conquer(Agent.AttackPoints, Agent.TeamID);
        else
            Substate = Substates.Exit;
    }
}
