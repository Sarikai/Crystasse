using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

[BurstCompile]
struct IdleJob : IJobForEach<IdleData, LocalToWorld, MoveData>
{
    public float dt;

    public void Execute(ref IdleData c1, ref LocalToWorld c2, ref MoveData c3)
    {
        bool moveUp = (c1.YDirection == 1f);
        bool moveDown = (c1.YDirection == -1f);

        if(moveUp)
            c3.Direction = new float3(0, 1f, 0);
        else if(moveDown)
            c3.Direction = new float3(0, -1f, 0);

        if(moveUp && c2.Position.y >= Constants.MAX_UNIT_DISPLACEMENT ||
           moveDown && c2.Position.y <= -Constants.MAX_UNIT_DISPLACEMENT)
            c1.YDirection *= -1f;
    }
}
