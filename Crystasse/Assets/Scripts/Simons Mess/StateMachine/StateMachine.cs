using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

public static class StateMachine
{
    public static float DeltaTime { get; private set; }
    private static readonly List<AttackState> _attackStates = new List<AttackState>();
    private static readonly List<BuildState> _buildStates = new List<BuildState>();
    private static readonly List<ConquerState> _conquerStates = new List<ConquerState>();
    private static readonly List<IdleState> _idleStates = new List<IdleState>();
    private static readonly List<MoveState> _moveStates = new List<MoveState>();

    public static List<AttackState> AttackStates => _attackStates;
    public static List<BuildState> BuildStates => _buildStates;
    public static List<ConquerState> ConquerStates => _conquerStates;
    public static List<IdleState> IdleStates => _idleStates;
    public static List<MoveState> MoveStates => _moveStates;

    public static JobHandle AttackJob { get; private set; }
    public static JobHandle BuildJob { get; private set; }
    public static JobHandle ConquerJob { get; private set; }
    public static JobHandle IdleJob { get; private set; }
    private static TransformAccessArray AccessArray = new TransformAccessArray();
    public static JobHandle MoveJob { get; private set; }

    public static void Update()
    {
        DeltaTime = Time.deltaTime;

        UpdateStates();

        //ScheduleJobs();
    }

    private static void UpdateStates()
    {
        foreach(var state in _attackStates)
            state.UpdateState();
        foreach(var state in _buildStates)
            state.UpdateState();
        foreach(var state in _conquerStates)
            state.UpdateState();
        foreach(var state in _idleStates)
            state.UpdateState();
        foreach(var state in _moveStates)
            state.UpdateState();
    }

    private static void ScheduleJobs()
    {
        WaitOnJobs();
        AttackJob = new AttackStatesJob().Schedule(AttackStates.Count, AttackStates.Count);
        BuildJob = new BuildStatesJob().Schedule(BuildStates.Count, BuildStates.Count);
        ConquerJob = new ConquerStatesJob().Schedule(ConquerStates.Count, ConquerStates.Count);
        IdleJob = new IdleStatesJob().Schedule(AccessArray);
        MoveJob = new MoveStatesJob().Schedule(MoveStates.Count, MoveStates.Count);
    }

    private static void WaitOnJobs()
    {
        AttackJob.Complete();
        BuildJob.Complete();
        ConquerJob.Complete();
        IdleJob.Complete();
        MoveJob.Complete();
    }

    public static void SwitchState(Agent agent, State newState)
    {
        if(agent.CurrentState != null)
            RemoveState(agent.CurrentState);
        agent.CurrentState = newState;
        AddState(newState);
        //if(newState.Type == States.Idle)
        //    AccessArray.SetTransforms(new Transform[1] { agent.transform });
    }

    private static void RemoveState(State state)
    {
        switch(state.Type)
        {
            case States.Idle:
                var idle = state as IdleState;
                if(IdleStates.Contains(idle))
                    IdleStates.Remove(idle);
                break;
            case States.Build:
                var build = state as BuildState;
                if(BuildStates.Contains(build))
                    BuildStates.Remove(build);
                break;
            case States.Attack:
                var attack = state as AttackState;
                if(AttackStates.Contains(attack))
                    AttackStates.Remove(attack);
                break;
            case States.Conquer:
                var conquer = state as ConquerState;
                if(ConquerStates.Contains(conquer))
                    ConquerStates.Remove(conquer);
                break;
            case States.Move:
                var move = state as MoveState;
                if(MoveStates.Contains(move))
                    MoveStates.Remove(move);
                break;
        }
    }

    private static void AddState(State state)
    {
        switch(state.Type)
        {
            case States.Idle:
                var idle = state as IdleState;
                IdleStates.Add(idle);
                break;
            case States.Build:
                var build = state as BuildState;
                BuildStates.Add(build);
                break;
            case States.Attack:
                var attack = state as AttackState;
                AttackStates.Add(attack);
                break;
            case States.Conquer:
                var conquer = state as ConquerState;
                ConquerStates.Add(conquer);
                break;
            case States.Move:
                var move = state as MoveState;
                MoveStates.Add(move);
                break;
        }
    }
}