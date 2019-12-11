using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateMachine
{
    private static readonly List<State> _attackStates = new List<State>();
    private static readonly List<State> _buildStates = new List<State>();
    private static readonly List<State> _conquerStates = new List<State>();
    private static readonly List<State> _idleStates = new List<State>();
    private static readonly List<State> _moveStates = new List<State>();

    public static List<State> AttackStates => _attackStates;
    public static List<State> BuildStates => _buildStates;
    public static List<State> ConquerStates => _conquerStates;
    public static List<State> IdleStates => _idleStates;
    public static List<State> MoveStates => _moveStates;
    private static State[] AllStatesWithoutIdle
    {
        get
        {
            List<State> ret = new List<State>();
            ret.AddRange(_attackStates);
            ret.AddRange(_buildStates);
            ret.AddRange(_conquerStates);
            ret.AddRange(_moveStates);

            return ret.ToArray();
        }
    }
    private static State[] AllStates
    {
        get
        {
            var ret = new List<State>(AllStatesWithoutIdle);
            ret.AddRange(_idleStates);

            return ret.ToArray();
        }
    }

    public static void Update()
    {
        UpdateStates();

        CleanStates();
    }

    private static void CleanStates()
    {
        List<Unit> agentsToIdle = new List<Unit>();
        var states = AllStatesWithoutIdle;

        foreach(var state in states)
            if(state.Completed)
                agentsToIdle.Add(state.Agent);

        foreach(var agent in agentsToIdle)
            if(agent != null)
                SwitchState(agent, new IdleState(agent, agent.MoveSpeed));
    }

    private static void UpdateStates()
    {
        var states = AllStates;
        foreach(var state in states)
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
                if(IdleStates.Contains(state))
                    IdleStates.Remove(state);
                break;
            case States.Build:
                if(BuildStates.Contains(state))
                    BuildStates.Remove(state);
                break;
            case States.Attack:
                if(AttackStates.Contains(state))
                    AttackStates.Remove(state);
                break;
            case States.Conquer:
                if(ConquerStates.Contains(state))
                    ConquerStates.Remove(state);
                break;
            case States.Move:
                if(MoveStates.Contains(state))
                    MoveStates.Remove(state);
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