using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateMachine
{
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

    public static void Update()
    {
        UpdateStates();
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

    public static void SwitchState(Agent agent, State newState)
    {
        if(agent == null)
            return;
        if(agent.CurrentState != null)
            RemoveState(agent.CurrentState);
        agent.CurrentState = newState;
        AddState(newState);
    }

    public static void RemoveState(State state)
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

    public static void AddState(State state)
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