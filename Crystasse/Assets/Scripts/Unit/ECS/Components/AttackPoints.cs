using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public struct AttackPoints : IComponentData
{
    public byte Value;
}
