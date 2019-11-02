using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

[/*BurstCompile ,*/ System.Serializable]
public class AttackSystem : JobComponentSystem
{
    public static EntityQueryBuilder query;

    //[BurstCompile]
    struct AttackJob : IJobForEachWithEntity<AttackPoints, State, Translation, Target, MoveSpeed, AttackRange>
    {
        public void Execute(Entity entity, int index, [ReadOnly] ref AttackPoints c0, [ReadOnly]ref State c1, ref Translation c2, ref Target c3, [ReadOnly] ref MoveSpeed c4, [ReadOnly] ref AttackRange c5)
        {
            if(c1.Value == States.Attack)
            {
                if(math.distancesq(c2.Value, c3.Value) >= c5.Value)
                    c2.Value += math.normalize(c3.Value - c2.Value);
                else
                {
                    //TODO: Make Burst-compilable. Maybe Arrays with HP, ID and AP?
                    query = query.WithAll<HealthPoints, ID, AttackPoints>();

                    bool attacked = false;

                    var dmg = c0.Value;

                    query.ForEach((Entity e, ref HealthPoints hp, ref ID id, ref AttackPoints ap, ref Target targetID, ref HealthPoints thisHP) =>
                    {
                        if(!attacked && !e.Equals(entity) && id.Value == targetID.ID)
                        {
                            hp.Value -= dmg;
                            thisHP.Value -= ap.Value;
                            attacked = true;
                        }
                    });
                }
            }
        }
    }

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
