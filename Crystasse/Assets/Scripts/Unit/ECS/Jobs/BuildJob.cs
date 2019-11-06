using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;

[BurstCompile]
internal struct BuildJob : IJobForEachWithEntity<BuildPoints, BuildData, BuildRange, Translation, TargetPos>
{
    public void Execute(Entity entity,
                        int index,
                        ref BuildPoints c0,
                        ref BuildData c1,
                        [ReadOnly] ref BuildRange c2,
                        ref Translation c4,
                        [ReadOnly] ref TargetPos c5)
    {
        //throw new System.NotImplementedException();
    }
}
