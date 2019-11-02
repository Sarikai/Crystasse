using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup)), BurstCompile]
public class TransitionSystem : JobComponentSystem
{
    private static EntityCommandBuffer.Concurrent buffer;
    struct TransitionJob : IJobForEachWithEntity<State, Translation, Target, AttackRange, BuildRange, ConquerRange>
    {
        public void Execute(Entity entity, int index, ref State c0, ref Translation c1, ref Target c2, [ReadOnly] ref AttackRange c3, [ReadOnly]ref BuildRange c4, [ReadOnly]ref ConquerRange c5)
        {
            if(TransitionRules.TransitionToAttack(c3, c1, c2))
            {
                if(c0.Value != States.Attack)
                {
                    RemoveStateData(c0.Value, entity, index);
                    buffer.AddComponent(index, entity, UnitData.DefaultAttackData);
                    c0.Value = States.Attack;
                }
            }
            else if(TransitionRules.TransitionToBuild(c4, c1, c2))
            {
                if(c0.Value != States.Build)
                {
                    RemoveStateData(c0.Value, entity, index);
                    buffer.AddComponent(index, entity, UnitData.DefaultBuildData);
                    c0.Value = States.Build;
                }
            }
            else if(TransitionRules.TransitionToConquer(c5, c1, c2))
            {
                if(c0.Value != States.Conquer)
                {
                    RemoveStateData(c0.Value, entity, index);
                    c0.Value = States.Conquer;
                    buffer.AddComponent(index, entity, UnitData.DefaultConquerData);
                }
            }
            else
            {
                if(c0.Value != States.Idle)
                {
                    RemoveStateData(c0.Value, entity, index);
                    buffer.AddComponent(index, entity, UnitData.DefaultIdleData);
                    c0.Value = States.Idle;
                }
            }
        }

        private void RemoveStateData(States type, Entity e, int jobIndex)
        {
            switch(type)
            {
                case States.Idle:
                    buffer.RemoveComponent<IdleData>(jobIndex, e);
                    return;
                case States.Build:
                    buffer.RemoveComponent<BuildData>(jobIndex, e);
                    return;
                case States.Attack:
                    buffer.RemoveComponent<AttackData>(jobIndex, e);
                    return;
                case States.Conquer:
                    buffer.RemoveComponent<ConquerData>(jobIndex, e);
                    return;
            }
        }
    }
    protected override void OnCreate()
    {
        base.OnCreate();
        buffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer().ToConcurrent();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new TransitionJob();

        return job.Schedule(this, inputDeps);
    }
}