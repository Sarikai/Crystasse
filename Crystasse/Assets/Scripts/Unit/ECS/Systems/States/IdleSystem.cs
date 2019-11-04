using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;

[BurstCompile, System.Serializable]
public class IdleSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new IdleJob() { dt = Time.deltaTime };

        return job.Schedule(this, inputDeps);
    }
}
