using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct ID : IComponentData
{
    public int Value;
}
