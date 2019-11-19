using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
internal struct ConquerJob : IJobForEachWithEntity<AttackPoints, ConquerData, LocalToWorld, MoveData>
{
    public void Execute(Entity entity, int index, ref AttackPoints c0, ref ConquerData c1, ref LocalToWorld c4, ref MoveData c5)
    {
        //throw new System.NotImplementedException();
    }
}
