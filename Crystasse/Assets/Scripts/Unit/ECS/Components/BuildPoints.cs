using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public struct BuildPoints : IComponentData
{
    public byte Value;
}
