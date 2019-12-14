using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public Unit Agent { get; protected set; }
    public States Type { get; protected set; }
    public Substates Substate { get; protected set; }
    public bool Completed { get; protected set; }

    public virtual void UpdateState()
    {
        switch(Substate)
        {
            case Substates.Enter:
                Enter();
                break;
            case Substates.Stay:
                Stay();
                break;
            case Substates.Exit:
                Exit();
                break;
            default:
                break;
        }
    }

    protected abstract void Enter();
    protected abstract void Stay();
    protected abstract void Exit();
}
