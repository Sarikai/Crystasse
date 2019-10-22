using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public struct BuildSpeed : IComponentData
{
    public float Value;
}
