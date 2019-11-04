using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile, System.Serializable]
public class ConquerSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ConquerJob();

        return job.Schedule(this, inputDeps);
    }
}
