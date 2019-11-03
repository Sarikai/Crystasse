using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct ConquerRange : IComponentData
{
    public float Value;
}
