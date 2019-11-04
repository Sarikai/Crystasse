using Unity.Entities;

[ExcludeComponent(typeof(AttackData), typeof(BuildData), typeof(ConquerData))]
public struct IdleTransitionJob : IJobForEachWithEntity<State>
{
    public void Execute(Entity entity, int index, ref State c0)
    {
        if(c0.Value != States.Idle)
        {
            TransitionSystem.buffer.AddComponent(index, entity, UnitData.DefaultIdleData);
            c0.Value = States.Idle;
        }
    }
}
