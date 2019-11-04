using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[UpdateInGroup(typeof(LateSimulationSystemGroup)), BurstCompile]
public abstract class TransitionSystem : JobComponentSystem
{
    internal static EntityCommandBuffer.Concurrent buffer;
    protected override void OnCreate()
    {
        base.OnCreate();
        buffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer().ToConcurrent();
    }

    protected override abstract JobHandle OnUpdate(JobHandle inputDeps);
}