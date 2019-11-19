using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateInGroup(typeof(TransformSystemGroup))]
public class MoveSystem : JobComponentSystem
{
    protected EndSimulationEntityCommandBufferSystem bufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        bufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new MoveJob().Schedule(this, inputDeps);
    }
}
