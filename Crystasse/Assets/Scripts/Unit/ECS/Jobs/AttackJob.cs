using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

//[BurstCompile]
struct AttackJob : IJobForEachWithEntity<AttackPoints, LocalToWorld, AttackData, MoveSpeed, AttackRange>
{
    public void Execute(Entity entity,
                int index,
                [ReadOnly] ref AttackPoints c0,
                ref LocalToWorld c2,
                ref AttackData c3,
                [ReadOnly] ref MoveSpeed c4,
                [ReadOnly] ref AttackRange c5)
    {
        if(math.distancesq(c2.Position, c3.TargetPos) >= c5.Value)
            //c2.Value += math.mul(new float4(math.normalize(c3.TargetPos - c2.Position), 0f),float4x4.identity)
            return;
        else
        {

            AttackPoints thisAp = c0;
            AttackData data = c3;
            HealthPoints thisHp = AttackSystem.world.EntityManager.GetComponentObject<HealthPoints>(entity);

            //TODO: Make Burst-compilable. Maybe Arrays with HP, ID and AP?
            AttackSystem.query = AttackSystem.query.WithAll<ID, HealthPoints, AttackPoints>();

            AttackSystem.query.ForEach(
                (ref ID id, ref HealthPoints hp, ref AttackPoints ap) =>
                {
                    if(AttackIfTargetEntity(ref id, ref hp, ref ap, ref data, ref thisAp, ref thisHp))
                        return;
                }
            );
        }
    }


    private static bool AttackIfTargetEntity([ReadOnly]ref ID id,
                                      ref HealthPoints hp,
                                      [ReadOnly] ref AttackPoints ap,
                                      ref AttackData data,
                                      ref AttackPoints thisAp,
                                      ref HealthPoints thisHp)
    {
        if(id.Equals(data.TargetID))
        {
            hp.Value -= thisAp.Value;
            thisHp.Value -= ap.Value;
            return true;
        }

        return false;
    }
}
