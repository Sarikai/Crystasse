using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
struct ConquerJob : IJobForEachWithEntity<AttackPoints, ConquerData, ConquerRange, State, Translation, MoveSpeed>
{
    public void Execute(Entity entity, int index, ref AttackPoints c0, ref ConquerData c1, ref ConquerRange c2, ref State c3, ref Translation c4, ref MoveSpeed c5)
    {
        //throw new System.NotImplementedException();
    }
}
