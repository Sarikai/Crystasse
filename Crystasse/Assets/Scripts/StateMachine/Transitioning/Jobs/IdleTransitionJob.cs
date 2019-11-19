using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[ExcludeComponent(typeof(AttackData), typeof(BuildData), typeof(ConquerData))]
public struct IdleTransitionJob : IJobForEachWithEntity<ID>
{
    [Unity.Collections.LowLevel.Unsafe.NativeSetThreadIndex]
    private int _threadIndex;

    public EntityCommandBuffer.Concurrent buffer;

    public void Execute(Entity entity, int index, [ReadOnly] ref ID c0)
    {
        TransitionSystem.RemoveStateData(entity, _threadIndex, ref buffer);
        buffer.AddComponent(_threadIndex, entity, UnitData.DefaultIdleData);
    }
}
