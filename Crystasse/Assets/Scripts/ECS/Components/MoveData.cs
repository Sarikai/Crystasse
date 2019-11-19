using Unity.Entities;
using Unity.Mathematics;

[System.Serializable]
public struct MoveData : IComponentData
{
    public float MoveSpeed;
    public float3 Direction;

    public float3 MovementVector => Direction * MoveSpeed;

    public MoveData(float moveSpeed)
    {
        MoveSpeed = moveSpeed;
        Direction = new float3();
    }
}
