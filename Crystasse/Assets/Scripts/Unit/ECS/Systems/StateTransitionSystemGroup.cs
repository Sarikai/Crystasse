using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))/*, BurstCompile*/]
public class StateTransitionSystemGroup : ComponentSystemGroup
{
    public override IEnumerable<ComponentSystemBase> Systems => base.Systems;

    public override void SortSystemUpdateList()
    {
        base.SortSystemUpdateList();
    }
}
