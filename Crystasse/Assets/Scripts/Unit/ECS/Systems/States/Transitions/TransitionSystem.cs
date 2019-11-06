using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[UpdateInGroup(typeof(StateTransitionSystemGroup)), BurstCompile]
public abstract class TransitionSystem : JobComponentSystem
{
    protected EndSimulationEntityCommandBufferSystem bufferSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        bufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override abstract JobHandle OnUpdate(JobHandle inputDeps);

    internal static void RemoveStateData(Entity e, int jobIndex, ref EntityCommandBuffer.Concurrent buffer)
    {
        var world = World.Active;
        if(world.EntityManager.HasComponent<IdleData>(e))
            buffer.RemoveComponent<IdleData>(jobIndex, e);
        if(world.EntityManager.HasComponent<BuildData>(e))
            buffer.RemoveComponent<BuildData>(jobIndex, e);
        if(world.EntityManager.HasComponent<AttackData>(e))
            buffer.RemoveComponent<AttackData>(jobIndex, e);
        if(world.EntityManager.HasComponent<ConquerData>(e))
            buffer.RemoveComponent<ConquerData>(jobIndex, e);
    }
    internal static void RemoveStateData(Entity e, EntityManager manager)
    {
        if(manager == null)
            return;
        if(manager.HasComponent<IdleData>(e))
            manager.RemoveComponent<IdleData>(e);
        if(manager.HasComponent<BuildData>(e))
            manager.RemoveComponent<BuildData>(e);
        if(manager.HasComponent<AttackData>(e))
            manager.RemoveComponent<AttackData>(e);
        if(manager.HasComponent<ConquerData>(e))
            manager.RemoveComponent<ConquerData>(e);
    }
    public static void SwitchState(Entity entity, States cmd)
    {
        if(World.Active.EntityManager.Exists(entity))
        {
            var manager = World.Active.EntityManager;
            RemoveStateData(entity, manager);

            switch(cmd)
            {
                case States.Idle:
                    manager.AddComponent<IdleData>(entity);
                    break;
                case States.Build:
                    manager.AddComponent<BuildData>(entity);
                    break;
                case States.Attack:
                    manager.AddComponent<AttackData>(entity);
                    break;
                case States.Conquer:
                    manager.AddComponent<ConquerData>(entity);
                    break;
            }
        }
    }
}