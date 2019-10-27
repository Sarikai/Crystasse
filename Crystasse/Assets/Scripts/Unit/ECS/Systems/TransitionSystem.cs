using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
public class TransitionSystem : JobComponentSystem
{
    public static World world = World.Active;

    struct TransitionJob : IJobForEachWithEntity<State, Substate, Translation, Target>
    {
        public void Execute(Entity entity, int index, ref State c0, ref Substate c1, [ReadOnly] ref Translation c2, [ReadOnly] ref Target c3)
        {
            if(TransitionRules.TransitionToAttack(world.EntityManager.GetComponentData<AttackRange>(entity), c2, c3))
            {
                if(c0.Value != States.Attack)
                {
                    c0.Value = States.Attack;
                    c1.Value = SubStates.Enter;
                    world.EntityManager.AddComponent<AttackData>(entity);
                }
            }
            else if(TransitionRules.TransitionToBuild(world.EntityManager.GetComponentData<BuildRange>(entity), c2, c3))
            {
                if(c0.Value != States.Build)
                {
                    c0.Value = States.Build;
                    c1.Value = SubStates.Enter;
                    world.EntityManager.AddComponent<BuildData>(entity);
                }
            }
            else if(TransitionRules.TransitionToConquer(world.EntityManager.GetComponentData<ConquerRange>(entity), c2, c3))
            {
                if(c0.Value != States.Conquer)
                {
                    c0.Value = States.Conquer;
                    c1.Value = SubStates.Enter;
                    world.EntityManager.AddComponent<ConquerData>(entity);
                }
            }
            else if(c0.Value != States.Idle)
            {
                c0.Value = States.Idle;
                c1.Value = SubStates.Enter;
                world.EntityManager.AddComponent<IdleData>(entity);
            }

            RemoveIncorrectStateData(entity, ref c0);
        }

        private void RemoveIncorrectStateData(Entity entity, [ReadOnly] ref State c0)
        {
            if(c0.Value != States.Idle && world.EntityManager.HasComponent<IdleData>(entity))
                world.EntityManager.RemoveComponent<IdleData>(entity);
            if(c0.Value != States.Attack && world.EntityManager.HasComponent<AttackData>(entity))
                world.EntityManager.RemoveComponent<AttackData>(entity);
            if(c0.Value != States.Conquer && world.EntityManager.HasComponent<ConquerData>(entity))
                world.EntityManager.RemoveComponent<ConquerData>(entity);
            if(c0.Value != States.Build && world.EntityManager.HasComponent<BuildData>(entity))
                world.EntityManager.RemoveComponent<BuildData>(entity);
        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new TransitionJob();

        return job.Schedule(this, inputDeps);
    }
}
