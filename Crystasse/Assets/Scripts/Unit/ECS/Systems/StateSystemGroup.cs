using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;

//[BurstCompile]
public class StateSystemGroup : ComponentSystemGroup
{
    public override IEnumerable<ComponentSystemBase> Systems => base.Systems;

    public override void SortSystemUpdateList()
    {
        base.SortSystemUpdateList();
    }
}
