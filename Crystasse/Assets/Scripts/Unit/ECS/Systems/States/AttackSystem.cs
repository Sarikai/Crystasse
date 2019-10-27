using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;

[DisableAutoCreation]
public class AttackSystem : JobComponentSystem
{
    [BurstCompile]
    struct AttackJob : IJobForEach<AttackPoints, State, Translation, Target, MoveSpeed>
    {
        public float Range;
        public void Execute([ReadOnly]ref AttackPoints c0, ref State c1, ref Translation c2, ref Target c3, [ReadOnly]ref MoveSpeed c4)
        {
            //throw new System.NotImplementedException();
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AttackJob() { Range = 0f };

        return job.Schedule(this, inputDeps);
    }
}
