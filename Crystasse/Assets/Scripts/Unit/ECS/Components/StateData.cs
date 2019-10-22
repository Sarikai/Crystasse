using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public struct StateData : IComponentData
{
    public UnitCommand Command;
    public SubStates SubState;
}