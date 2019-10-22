using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public struct Target : IComponentData
{
    public uint ID;
    public float3 Value;
}