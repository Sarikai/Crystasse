using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
public class TransitionSystem : JobComponentSystem
{
    public static readonly World world = World.Active;

    //[BurstCompile]
    struct TransitionJob : IJobForEachWithEntity<State, Translation, Target>
    {
        public void Execute(Entity entity, int index, ref State c1, [ReadOnly] ref Translation c2, [ReadOnly] ref Target c3)
        {
            if(TransitionRules.TransitionToAttack(world.EntityManager.GetComponentData<AttackRange>(entity), c2, c3))
            {
                if(c1.Value != States.Attack)
                    c1.Value = States.Attack;
            }
            else if(TransitionRules.TransitionToBuild(world.EntityManager.GetComponentData<BuildRange>(entity), c2, c3))
            {
                if(c1.Value != States.Build)
                    c1.Value = States.Build;
            }
            else if(TransitionRules.TransitionToConquer(world.EntityManager.GetComponentData<ConquerRange>(entity), c2, c3))
            {
                if(c1.Value != States.Conquer)
                    c1.Value = States.Conquer;
            }
            else
                c1.Value = States.Idle;
        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new TransitionJob();

        return job.Schedule(this, inputDeps);
    }
}

[DisableAutoCreation]
public class TransitionCSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref State c1, ref Translation c2, ref Target c3) =>
        {
            RemoveIncorrectStateData(entity, ref c1);
            AddNeededStateData(entity, ref c1);
        }
    );
    }
    private void RemoveIncorrectStateData(Entity entity, [ReadOnly] ref State c0)
    {
        if(c0.Value != States.Idle && World.EntityManager.HasComponent<IdleData>(entity))
            World.EntityManager.RemoveComponent<IdleData>(entity);
        if(c0.Value != States.Attack && World.EntityManager.HasComponent<AttackData>(entity))
            World.EntityManager.RemoveComponent<AttackData>(entity);
        if(c0.Value != States.Conquer && World.EntityManager.HasComponent<ConquerData>(entity))
            World.EntityManager.RemoveComponent<ConquerData>(entity);
        if(c0.Value != States.Build && World.EntityManager.HasComponent<BuildData>(entity))
            World.EntityManager.RemoveComponent<BuildData>(entity);
    }

    private void AddNeededStateData(Entity entity, [ReadOnly] ref State c0)
    {
        if(c0.Value == States.Idle && !World.EntityManager.HasComponent<IdleData>(entity))
        {
            World.EntityManager.AddComponent<IdleData>(entity);
            World.EntityManager.SetComponentData<IdleData>(entity, new IdleData() { YDirection = 1f });
        }
        if(c0.Value == States.Attack && !World.EntityManager.HasComponent<AttackData>(entity))
            World.EntityManager.AddComponent<AttackData>(entity);
        if(c0.Value == States.Conquer && !World.EntityManager.HasComponent<ConquerData>(entity))
            World.EntityManager.AddComponent<ConquerData>(entity);
        if(c0.Value == States.Build && !World.EntityManager.HasComponent<BuildData>(entity))
            World.EntityManager.AddComponent<BuildData>(entity);
    }
}
