using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct MoveSpeed : IComponentData
{
    public float Value;
}
