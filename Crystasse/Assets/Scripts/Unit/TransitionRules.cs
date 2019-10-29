using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public static class TransitionRules
{
    static TransitionRules()
    {

    }

    public static bool TransitionToAttack([ReadOnly]AttackRange range, [ReadOnly] Translation pos, [ReadOnly] Target target) =>
        range.Value > 0f && math.distancesq(pos.Value, target.Value) <= range.Value;

    public static bool TransitionToBuild([ReadOnly] BuildRange range, [ReadOnly]Translation pos, [ReadOnly]Target target) =>
        range.Value > 0f && math.distancesq(pos.Value, target.Value) <= range.Value;

    public static bool TransitionToConquer([ReadOnly] ConquerRange range, [ReadOnly]Translation pos, [ReadOnly]Target target) =>
        range.Value > 0f && math.distancesq(pos.Value, target.Value) <= range.Value;
}
