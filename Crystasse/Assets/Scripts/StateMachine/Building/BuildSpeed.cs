using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct BuildSpeed : IComponentData
{
    public float Value;
}
