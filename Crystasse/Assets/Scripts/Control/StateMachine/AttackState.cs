using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public Unit Target { get; private set; }

    float _curveX = 0;
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
        Target.TakeDamage(Agent.AttackPoints);
        //TODO: explosion effects here would be nice
        Completed = true;
    }

    protected override void Stay()
    {
        _curveX += Time.deltaTime;
        if(_curveX >= 1)
        {
            _curveX = 0;
            Substate = Substates.Exit;
        }
        else
            Agent.PlayAttackAnim(Target.Rigidbody.position - Agent.Rigidbody.position, _curveX);
    }
}
