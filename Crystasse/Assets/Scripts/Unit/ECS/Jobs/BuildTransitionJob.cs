using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

struct BuildTransitionJob : IJobForEachWithEntity<State, Translation, TargetPos, BuildRange>
{
    public void Execute(Entity entity, int index, ref State c0, [ReadOnly] ref Translation c1, [ReadOnly] ref TargetPos c2, [ReadOnly] ref BuildRange c3)
    {
        if(TransitionRules.TransitionToBuild(c3, c1, c2.Value))
        {
            if(c0.Value != States.Build)
            {
                RemoveStateData(c0.Value, entity, index);
                TransitionSystem.buffer.AddComponent(index, entity, UnitData.DefaultBuildData);
                c0.Value = States.Build;
            }
        }
    }

    private void RemoveStateData(States type, Entity e, int jobIndex)
    {
        switch(type)
        {
            case States.Idle:
                TransitionSystem.buffer.RemoveComponent<IdleData>(jobIndex, e);
                return;
            case States.Build:
                TransitionSystem.buffer.RemoveComponent<BuildData>(jobIndex, e);
                return;
            case States.Attack:
                TransitionSystem.buffer.RemoveComponent<AttackData>(jobIndex, e);
                return;
            case States.Conquer:
                TransitionSystem.buffer.RemoveComponent<ConquerData>(jobIndex, e);
                return;
        }
    }
}