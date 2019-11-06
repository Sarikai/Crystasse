using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
internal struct ConquerJob : IJobForEachWithEntity<AttackPoints, ConquerData, ConquerRange, LocalToWorld, MoveSpeed>
{
    public void Execute(Entity entity, int index, ref AttackPoints c0, ref ConquerData c1, ref ConquerRange c2, ref LocalToWorld c4, ref MoveSpeed c5)
    {
        //throw new System.NotImplementedException();
    }
}
