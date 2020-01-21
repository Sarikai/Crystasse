using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Jobs;

public class MoveState : State
{
    public float Speed;
    public Rigidbody Rigidbody;
    public Vector3 Destination;

    private float _timer = 0f;

    public NavMeshAgent MeshAgent;

    public MoveState(float speed, Unit agent, Vector3 destination)
    {
        Agent = agent;
        Type = States.Move;
        Speed = speed;
        Rigidbody = Agent.Rigidbody;
        Destination = destination;
    }
    public MoveState(float speed, Unit agent, Vector3 destination, NavMeshAgent meshAgent)
    {
        Agent = agent;
        Type = States.Move;
        Speed = speed;
        MeshAgent = meshAgent;
        Destination = destination;
    }

    protected override void Enter()
    {
        Substate = Substates.Stay;
        MeshAgent.SetDestination(Destination);
        Stay();
    }

    protected override void Exit()
    {
        Completed = true;
    }

    protected override void Stay()
    {
        if(MeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            Substate = Substates.Exit;
        else
            MoveTowardsDest();
    }

    private void MoveTowardsDest()
    {
        Agent.PlayMoveAnim(_timer);
    }
}
