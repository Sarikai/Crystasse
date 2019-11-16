using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile, System.Serializable]
public struct AttackData : IComponentData
{
    public int TargetID;
    public float3 TargetPos;

    public AttackData(int targetID = -1)
    {
        TargetID = targetID;
        TargetPos = new float3();
    }
}
