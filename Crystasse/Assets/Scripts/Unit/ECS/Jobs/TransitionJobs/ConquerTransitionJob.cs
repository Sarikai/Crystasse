﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[ExcludeComponent(typeof(ConquerData))]
struct ConquerTransitionJob : IJobForEachWithEntity<LocalToWorld, ConquerRange>
{
    public EntityCommandBuffer.Concurrent buffer;

    public void Execute(Entity entity, int index, [ReadOnly] ref LocalToWorld c1, [ReadOnly] ref ConquerRange c3)
    {
        if(TransitionRules.ShouldTransition(c3.SqrValue, c1, c1.Position))
        {
            TransitionSystem.RemoveStateData(entity, index, ref buffer);
            buffer.AddComponent(index, entity, UnitData.DefaultConquerData);
        }
    }
}
