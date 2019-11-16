using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct HealthPoints : IComponentData
{
    public byte Value;
}
