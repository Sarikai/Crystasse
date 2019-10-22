using Unity.Entities;
using Unity.Burst;

[BurstCompile]

public struct ID : IComponentData
{
    public uint Value;
}
