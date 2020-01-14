using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class MoveState : State
{
    public float Speed;
    public Rigidbody Rigidbody;
    public Vector3 Destination;

    private float _timer = 0f;

    public MoveState(float speed, Unit agent, Vector3 destination)
    {
        Agent = agent;
        Type = States.Move;
        Speed = speed;
        Rigidbody = Agent.Rigidbody;
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
        if((Destination - Rigidbody.transform.position).sqrMagnitude <= 0.1f)
            Substate = Substates.Exit;
        else
            MoveTowardsDest();
    }

    private void MoveTowardsDest()
    {
        Vector3 direction = (Destination - Rigidbody.transform.position).normalized;

        //_timer += Time.deltaTime;

        //if(_timer >= 2f)
        //    _timer = 0f;

        Agent.PlayMoveAnim(_timer);

        Rigidbody.transform.position += direction * Speed * Time.deltaTime;
    }
}
