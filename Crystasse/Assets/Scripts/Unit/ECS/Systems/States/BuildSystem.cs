using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile, System.Serializable, UpdateInGroup(typeof(StateSystemGroup))]
public class BuildSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new BuildJob();

        return job.Schedule(this, inputDeps);
    }
}
