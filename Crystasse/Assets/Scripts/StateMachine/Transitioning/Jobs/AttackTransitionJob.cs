using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

//[ExcludeComponent(typeof(AttackData))]
struct AttackTransitionJob : ITriggerEventsJob /*IJobForEachWithEntity<LocalToWorld, Range>*/
{
    [Unity.Collections.LowLevel.Unsafe.NativeSetThreadIndex]
    private int _threadIndex;

    public EntityCommandBuffer.Concurrent buffer;

    public void Execute(TriggerEvent triggerEvent)
    {
        TransitionToAttack(triggerEvent.Entities.EntityA);

        TransitionToAttack(triggerEvent.Entities.EntityB);
    }

    private void TransitionToAttack(Entity e)
    {
        TransitionSystem.RemoveStateData(e, _threadIndex, ref buffer);
        var data = new AttackData();
        data.TargetID = World.Active.EntityManager.GetComponentData<ID>(e).Value;
        buffer.AddComponent(_threadIndex, e, data);
    }

    //public void Execute(Entity entity, int index, [ReadOnly] ref LocalToWorld c1, [ReadOnly] ref Range c3)
    //{
    //    if(TransitionRules.ShouldTransition(c3.SqrValue, c1, c1.Position))
    //    {
    //        TransitionSystem.RemoveStateData(entity, index, ref buffer);
    //        buffer.AddComponent(index, entity, UnitData.DefaultAttackData);
    //    }
    //}

}