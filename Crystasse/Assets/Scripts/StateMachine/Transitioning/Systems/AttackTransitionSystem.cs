using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[BurstCompile]
public class AttackTransitionSystem : TransitionSystem
{
    BuildPhysicsWorld physicsWorld;
    protected override void OnCreate()
    {
        base.OnCreate();

        physicsWorld = World.Active.GetExistingSystem<BuildPhysicsWorld>();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AttackTransitionJob() { buffer = bufferSystem.CreateCommandBuffer().ToConcurrent() }.Schedule(
            World.Active.GetExistingSystem<StepPhysicsWorld>().Simulation,
            ref physicsWorld.PhysicsWorld,
            inputDeps);

        bufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}
