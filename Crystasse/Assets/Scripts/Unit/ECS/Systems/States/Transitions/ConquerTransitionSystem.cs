using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

[BurstCompile]
public class ConquerTransitionSystem : TransitionSystem
{
    protected override void OnCreate()
    {
        base.OnCreate();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AttackTransitionJob() { buffer = bufferSystem.CreateCommandBuffer().ToConcurrent() }.Schedule(this, inputDeps);

        bufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}
