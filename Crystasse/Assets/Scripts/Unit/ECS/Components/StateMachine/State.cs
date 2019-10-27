using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct State : IComponentData
{
    public States Value;
}