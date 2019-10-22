using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public struct TeamID : ISharedComponentData
{
    public byte Value;
}
