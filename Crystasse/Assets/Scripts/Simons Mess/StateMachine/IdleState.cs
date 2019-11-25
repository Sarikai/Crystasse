using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class IdleState : State
{
    public Transform Transform;
    private float moveSPeedd;
    private float _timer = 0f;

    public IdleState(Transform transform, float moveSpeed)
    {
        Type = States.Idle;
        Transform = transform;
        moveSPeedd = moveSpeed;
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
        _timer += StateMachine.DeltaTime;

        Transform.position += new Vector3(0f, 1f, 0f) * math.sin(_timer) * moveSPeedd * StateMachine.DeltaTime;

        if(_timer >= 2f * math.PI)
            _timer = 0f;
    }
}
