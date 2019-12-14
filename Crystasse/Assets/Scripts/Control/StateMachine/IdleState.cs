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
        _timer += Time.deltaTime;

        Transform.position += new Vector3(0f, 1f, 0f) * math.sin(_timer) * _moveSpeed * Time.deltaTime;

        if(_timer >= 2f * math.PI)
            _timer = 0f;
    }
}
