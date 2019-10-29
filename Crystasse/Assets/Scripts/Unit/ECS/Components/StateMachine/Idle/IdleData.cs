using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct IdleData : IComponentData
{
    public float YDirection;
}
