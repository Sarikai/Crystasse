using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviourPunCallbacks
{
    public State CurrentState { get; set; }

    public override void OnEnable()
    {
        if (CurrentState != null)
            StateMachine.AddState(CurrentState);
    }

    public override void OnDisable()
    {
        if (CurrentState != null)
            StateMachine.RemoveState(CurrentState);
    }

    public void OnDestroy()
    {
        OnDisable();
    }

}
