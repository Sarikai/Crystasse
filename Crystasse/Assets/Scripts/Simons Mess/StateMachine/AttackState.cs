using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public Unit Agent { get; private set; }
    public Unit Target { get; private set; }

    public AttackState(Unit agent, Unit target)
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
