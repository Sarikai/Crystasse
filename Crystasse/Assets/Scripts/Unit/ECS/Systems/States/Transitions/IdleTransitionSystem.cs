using Unity.Entities;
using Unity.Jobs;

[UpdateAfter(typeof(AttackTransitionSystem)), UpdateAfter(typeof(ConquerTransitionSystem)), UpdateAfter(typeof(BuildTransitionSystem))]
public class IdleTransitionSystem : TransitionSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new IdleTransitionJob();

        return job.Schedule(this, inputDeps);
    }
}
