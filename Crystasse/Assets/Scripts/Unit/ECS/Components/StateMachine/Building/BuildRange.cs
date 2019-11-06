using Unity.Entities;
using Unity.Burst;

[BurstCompile, System.Serializable]
public struct BuildRange : IComponentData
{
    public float Value;
    public float SqrValue => Unity.Mathematics.math.mul(Value, Value);
}
