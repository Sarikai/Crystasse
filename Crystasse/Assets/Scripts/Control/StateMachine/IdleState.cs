using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class IdleState : State
{
    public Transform Transform;
    private float _moveSpeed;
    private float _timer = 0f;

    public IdleState(Unit agent, float moveSpeed)
    {
        Agent = agent;
        Type = States.Idle;
        Transform = Agent.transform;
        _moveSpeed = moveSpeed;
    }

    protected override void Enter()
    {
        Substate = Substates.Stay;
        Stay();
    }

    protected override void Exit()
    {
        Completed = true;
    }

    protected override void Stay()
    {
        Agent.PlayMoveAnim(_timer);
    }
}
