using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : State
{
    public Unit Agent;
    public Bridge Target;
    private float _timer = 0f;

    public BuildState(Unit agent, Bridge target)
    {
        Agent = agent ?? throw new ArgumentNullException(nameof(agent));
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }

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
