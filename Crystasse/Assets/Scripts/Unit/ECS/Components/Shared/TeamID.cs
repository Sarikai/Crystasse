using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct TeamID : ISharedComponentData
{
    public byte Value;
}
