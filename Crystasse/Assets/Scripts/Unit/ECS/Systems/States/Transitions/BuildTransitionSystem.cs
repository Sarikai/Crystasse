using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

[BurstCompile]
public class BuildTransitionSystem : TransitionSystem
{
    protected override void OnCreate()
    {
        base.OnCreate();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new BuildTransitionJob() { buffer = bufferSystem.CreateCommandBuffer().ToConcurrent() }.Schedule(this, inputDeps);

        bufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}
