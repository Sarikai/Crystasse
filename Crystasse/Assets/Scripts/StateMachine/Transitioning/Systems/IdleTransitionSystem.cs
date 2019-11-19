using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

[UpdateAfter(typeof(AttackTransitionSystem)), UpdateAfter(typeof(ConquerTransitionSystem)), UpdateAfter(typeof(BuildTransitionSystem)), BurstCompile]
public class IdleTransitionSystem : TransitionSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new IdleTransitionJob() { buffer = bufferSystem.CreateCommandBuffer().ToConcurrent() }.Schedule(this, inputDeps);

        bufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}
