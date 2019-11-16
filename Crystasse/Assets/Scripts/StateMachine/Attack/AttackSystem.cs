using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

[/*BurstCompile ,*/ System.Serializable, UpdateInGroup(typeof(StateSystemGroup))]
public class AttackSystem : JobComponentSystem
{
    public static EntityQueryBuilder query;
    public static readonly World world = World.Active;

    protected override void OnCreate()
    {
        base.OnCreate();
        query = new EntityQueryBuilder();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AttackJob();

        return job.Schedule(this, inputDeps);
    }
}
