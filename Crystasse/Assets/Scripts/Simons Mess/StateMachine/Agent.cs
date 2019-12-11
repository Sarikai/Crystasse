using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : Photon.Pun.MonoBehaviourPun
{
    public State CurrentState { get; set; }

    protected void OnEnable()
    {
        if(CurrentState != null)
            StateMachine.AddState(CurrentState);
    }

    protected void OnDisable()
    {
        if(CurrentState != null)
            StateMachine.RemoveState(CurrentState);
    }

    protected void OnDestroy()
    {
        OnDisable();
    }

}
