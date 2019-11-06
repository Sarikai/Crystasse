using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile, System.Serializable]
public struct AttackData : IComponentData
{
    public uint TargetID;
    public float3 TargetPos;
}
