using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;

//[ExcludeComponent(typeof(BuildData))]
struct BuildTransitionJob : ITriggerEventsJob /*IJobForEachWithEntity<LocalToWorld, Range>*/
{
    [Unity.Collections.LowLevel.Unsafe.NativeSetThreadIndex]
    private int _threadIndex;
    public EntityCommandBuffer.Concurrent buffer;

    public void Execute(TriggerEvent triggerEvent)
    {
        var eManager = World.Active.EntityManager;
        if(eManager.HasComponent<ID>(triggerEvent.Entities.EntityA))
            TransitionToBuild(triggerEvent.Entities.EntityB);
        else if(eManager.HasComponent<ID>(triggerEvent.Entities.EntityB))
            TransitionToBuild(triggerEvent.Entities.EntityA);
    }

    private void TransitionToBuild(Entity e)
    {
        TransitionSystem.RemoveStateData(e, _threadIndex, ref buffer);
        var data = new BuildData();
        data.BridgeID = World.Active.EntityManager.GetComponentData<BridgeID>(e).Value;
        buffer.AddComponent(_threadIndex, e, data);
    }

    //public void Execute(Entity entity, int index, [ReadOnly] ref LocalToWorld c1, [ReadOnly] ref Range c3)
    //{
    //    if(TransitionRules.ShouldTransition(c3.SqrValue, c1, c1.Position))
    //    {
    //        TransitionSystem.RemoveStateData(entity, index, ref buffer);
    //        buffer.AddComponent(index, entity, UnitData.DefaultBuildData);
    //    }
    //}
}