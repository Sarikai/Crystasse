using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;

[BurstCompile, System.Serializable]
public class IdleSystem : JobComponentSystem
{
    [BurstCompile]
    struct IdleJob : IJobForEach<IdleData, Translation, MoveSpeed>
    {
        public float dt;

        public void Execute(ref IdleData c1, ref Translation c2, [ReadOnly]ref MoveSpeed c3)
        {
            bool moveUp = (c1.YDirection == 1f);
            bool moveDown = (c1.YDirection == -1f);

            if(moveUp)
                c2.Value.y += math.mul(c3.Value, dt);
            else if(moveDown)
                c2.Value.y -= math.mul(c3.Value, dt);

            if(moveUp && c2.Value.y >= Constants.MAX_UNIT_DISPLACEMENT ||
               moveDown && c2.Value.y <= -Constants.MAX_UNIT_DISPLACEMENT)
                c1.YDirection *= -1f;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new IdleJob() { dt = Time.deltaTime };

        return job.Schedule(this, inputDeps);
    }
}
