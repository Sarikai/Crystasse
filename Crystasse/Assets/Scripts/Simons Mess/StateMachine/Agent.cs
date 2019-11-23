using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public abstract class Agent : MonoBehaviour
{
    public State CurrentState { get; protected set; }

}
