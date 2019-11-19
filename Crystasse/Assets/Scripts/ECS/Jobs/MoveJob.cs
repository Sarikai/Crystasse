using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public struct MoveJob : IJobForEach<MoveData, LocalToWorld, Translation>
{
    public void Execute([ReadOnly]ref MoveData c0, [ReadOnly] ref LocalToWorld c1, ref Translation c2)
    {
        c2.Value = c1.Position + c0.MovementVector;
    }
}
