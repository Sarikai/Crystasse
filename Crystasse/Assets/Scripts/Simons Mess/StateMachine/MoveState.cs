using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;

public class MoveState : State
{
    public float Speed;
    public Transform Transform;
    public float3 Destination;

    private float _timer = 0f;

    public MoveState(float speed, Unit agent, float3 destination)
    {
        Agent = agent;
        Type = States.Move;
        Speed = speed;
        Transform = Agent.transform;
        Destination = destination;
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
        if(math.distancesq(Destination, Transform.position) <= 0.1f)
            Substate = Substates.Exit;
        else
            MoveTowardsDest();
    }

    private void MoveTowardsDest()
    {
        float3 direction = math.normalize(Destination - (float3)Transform.position);

        _timer += Time.deltaTime;
        direction.y += math.sin(_timer);

        if(_timer >= 2 * math.PI)
            _timer = 0f;

        Transform.position += (Vector3)direction * Speed * Time.deltaTime;
    }
}
