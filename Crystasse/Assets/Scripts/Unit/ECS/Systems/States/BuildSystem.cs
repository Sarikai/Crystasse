using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;

public class BuildSystem : JobComponentSystem
{
    [BurstCompile]
    struct BuildJob : IJobForEach<BuildPoints, StateData, Translation, Target>
    {
        public float Range;

        public void Execute(ref BuildPoints c0, ref StateData c1, ref Translation c2, ref Target c3)
        {
            throw new System.NotImplementedException();
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new BuildJob() { Range = 0f };

        return job.Schedule(this, inputDeps);
    }
}
