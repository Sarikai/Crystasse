using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct TargetPos : IComponentData
{
    public float3 Value;
}