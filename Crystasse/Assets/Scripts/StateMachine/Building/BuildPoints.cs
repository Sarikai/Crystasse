using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct BuildPoints : IComponentData
{
    public byte Value;
}
