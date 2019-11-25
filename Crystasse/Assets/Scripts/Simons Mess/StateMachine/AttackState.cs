using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public UnitAccess Agent { get; private set; }
    public UnitAccess Target { get; private set; }

    public AttackState(UnitAccess agent, UnitAccess target)
    {
        Type = States.Attack;
        Agent = agent;
        Target = target;
    }

    protected override void Enter()
    {
        if(Target.Health > 0)
            Substate = Substates.Stay;
        else
            Substate = Substates.Exit;
    }

    protected override void Exit()
    {
        Completed = true;
    }

    protected override void Stay()
    {
        Target.TakeDamage(Agent.AttackPoints);
        Agent.TakeDamage(Target.AttackPoints);
    }
}
