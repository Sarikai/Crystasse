using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[ExcludeComponent(typeof(AttackData), typeof(BuildData), typeof(ConquerData))]
public struct IdleTransitionJob : IJobForEachWithEntity<LocalToWorld>
{
    public EntityCommandBuffer.Concurrent buffer;

    public void Execute(Entity entity, int index, [ReadOnly] ref LocalToWorld c0)
    {
        TransitionSystem.RemoveStateData(entity, index, ref buffer);
        buffer.AddComponent(index, entity, UnitData.DefaultIdleData);
    }
}
