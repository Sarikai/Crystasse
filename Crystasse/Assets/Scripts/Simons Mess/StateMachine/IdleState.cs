using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class IdleState : State
{
    public TransformAccess Transform;
    private float _timer = 0f;

    public IdleState(TransformAccess transform)
    {
        Type = States.Idle;
        Transform = transform;
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

        Transform.position += new Vector3(0f, 1f, 0f) * math.sin(_timer);

        if(_timer >= 2 * math.PI)
            _timer = 0f;
    }
}
