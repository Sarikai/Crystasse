using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct Target : IComponentData
{
    public uint ID;
    public float3 Value;
}