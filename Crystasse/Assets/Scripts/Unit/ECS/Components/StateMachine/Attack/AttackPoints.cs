using Unity.Entities;
using Unity.Burst;
using System;

[BurstCompile, Serializable]
public struct AttackPoints : IComponentData
{
    public byte Value;
}
