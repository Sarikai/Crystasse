using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

[BurstCompile]
struct IdleJob : IJobForEach<IdleData, LocalToWorld, MoveSpeed>
{
    public float dt;

    public void Execute(ref IdleData c1, ref LocalToWorld c2, [ReadOnly]ref MoveSpeed c3)
    {
        bool moveUp = (c1.YDirection == 1f);
        bool moveDown = (c1.YDirection == -1f);

        //if(moveUp)
        //    //c2.Value.y += math.mul(c3.Value, dt);
        //    return;
        //else if(moveDown)
        //    //c2.Value.y -= math.mul(c3.Value, dt);
        //    return;

        //if(moveUp && c2.Value.y >= Constants.MAX_UNIT_DISPLACEMENT ||
        //   moveDown && c2.Value.y <= -Constants.MAX_UNIT_DISPLACEMENT)
        //    c1.YDirection *= -1f;
    }
}
