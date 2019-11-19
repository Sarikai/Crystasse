using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

[BurstCompile]
public class ConquerTransitionSystem : TransitionSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ConquerTransitionJob() { buffer = bufferSystem.CreateCommandBuffer().ToConcurrent() }.Schedule(
            StepPhysicsWorld.Simulation,
            ref physicsWorld.PhysicsWorld,
            inputDeps);

        bufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}
