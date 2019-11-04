using Unity.Entities;
using Unity.Jobs;

public class AttackTransitionSystem : TransitionSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AttackTransitionJob();

        return job.Schedule(this, inputDeps);
    }
}
