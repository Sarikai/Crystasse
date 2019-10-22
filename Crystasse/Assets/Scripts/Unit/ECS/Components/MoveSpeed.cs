using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public struct MoveSpeed : IComponentData
{
    public float Value;
}
