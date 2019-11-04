using Unity.Entities;
using Unity.Jobs;

public class BuildTransitionSystem : TransitionSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new BuildTransitionJob();

        return job.Schedule(this, inputDeps);
    }
}
