using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

[/*BurstCompile ,*/ System.Serializable]
public class AttackSystem : JobComponentSystem
{
    public static EntityQueryBuilder query;
    public static World world;

    protected override void OnCreate()
    {
        base.OnCreate();
        query = new EntityQueryBuilder();
        world = World.Active;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AttackJob();

        return job.Schedule(this, inputDeps);
    }
}
