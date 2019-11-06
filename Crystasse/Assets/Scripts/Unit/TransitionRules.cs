using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public static class TransitionRules
{
    public static bool ShouldTransition([ReadOnly] float sqrRange, [ReadOnly] LocalToWorld localToWorld, [ReadOnly] float3 targetPos) =>
        sqrRange > 0f && math.distancesq(localToWorld.Position, targetPos) <= sqrRange;

    public static bool ShouldTransition([ReadOnly] LocalToWorld localToWorld, [ReadOnly] float range, [ReadOnly] float3 targetPos) =>
        ShouldTransition(math.mul(range, range), localToWorld, targetPos);
}
