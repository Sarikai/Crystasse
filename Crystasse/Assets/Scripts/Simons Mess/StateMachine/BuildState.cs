using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : State
{
    public UnitAccess Agent { get; private set; }
    public Bridge Target;
    private float _timer = 0f;


    protected override void Enter()
    {
        Substate = Substates.Stay;
    }

    protected override void Exit()
    {
        Completed = true;
    }

    protected override void Stay()
    {
        _timer += Time.deltaTime;

        if(_timer >= 1f && Agent.BuildPoints > 0)
        {
            Target.Build(Agent.BuildingPoints);
            _timer = 0f;
        }
    }
}
