using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;

public class IdleSystem : JobComponentSystem
{
    [BurstCompile]
    struct IdleJob : IJobForEach<StateData, Translation>
    {
        public void Execute(ref StateData c0, ref Translation c1)
        {
            throw new System.NotImplementedException();
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new IdleJob();

        return job.Schedule(this, inputDeps);
    }
}
