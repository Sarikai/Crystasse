using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;

public class ConquerSystem : JobComponentSystem
{
    [BurstCompile]
    struct ConquerJob : IJobForEach<AttackPoints, StateData, Translation, MoveSpeed>
    {
        public void Execute([ReadOnly]ref AttackPoints c0, ref StateData c1, ref Translation c2, [ReadOnly]ref MoveSpeed c3)
        {
            throw new System.NotImplementedException();
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ConquerJob();

        return job.Schedule(this, inputDeps);
    }
}
