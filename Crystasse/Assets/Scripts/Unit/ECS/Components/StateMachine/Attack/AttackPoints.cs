using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct AttackPoints : IComponentData
{
    public byte Value;
}