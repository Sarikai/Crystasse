using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public static class TransitionRules
{
    public static bool TransitionToAttack([ReadOnly]AttackRange range, [ReadOnly] Translation pos, [ReadOnly] float3 targetPos) =>
        range.SqrValue > 0f && math.distancesq(pos.Value, targetPos) <= range.SqrValue;

    public static bool TransitionToBuild([ReadOnly] BuildRange range, [ReadOnly]Translation pos, [ReadOnly]float3 targetPos) =>
        range.SqrValue > 0f && math.distancesq(pos.Value, targetPos) <= range.SqrValue;

    public static bool TransitionToConquer([ReadOnly] ConquerRange range, [ReadOnly]Translation pos, [ReadOnly]float3 targetPos) =>
        range.SqrValue > 0f && math.distancesq(pos.Value, targetPos) <= range.SqrValue;
}
