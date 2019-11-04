using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
struct BuildJob : IJobForEachWithEntity<BuildPoints, BuildData, BuildRange, State, Translation, TargetPos>
{
    public void Execute(Entity entity, int index, ref BuildPoints c0, ref BuildData c1, ref BuildRange c2, ref State c3, ref Translation c4, ref TargetPos c5)
    {
        //throw new System.NotImplementedException();
    }
}
