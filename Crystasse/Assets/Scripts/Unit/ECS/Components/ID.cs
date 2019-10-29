using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct ID : IComponentData
{
    public uint Value;
}
