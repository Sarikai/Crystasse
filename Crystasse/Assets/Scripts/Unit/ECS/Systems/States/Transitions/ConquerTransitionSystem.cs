using Unity.Entities;
using Unity.Jobs;

public class ConquerTransitionSystem : TransitionSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ConquerTransitionJob();

        return job.Schedule(this, inputDeps);
    }
}
