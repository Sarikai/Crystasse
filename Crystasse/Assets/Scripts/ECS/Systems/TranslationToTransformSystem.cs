using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Jobs;

public class TranslationToTransformSystem : JobComponentSystem
{
    TransformAccessArray transforms;
    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        //transforms.
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new TranslationToTransformJob() { }.Schedule(this, inputDeps);

        return job;
    }
}
