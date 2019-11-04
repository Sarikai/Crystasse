using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

//[BurstCompile]
struct AttackJob : IJobForEachWithEntity<AttackPoints, State, Translation, AttackData, MoveSpeed, AttackRange>
{
    public void Execute(Entity entity,
                int index,
                [ReadOnly] ref AttackPoints c0,
                [ReadOnly]ref State c1,
                ref Translation c2,
                ref AttackData c3,
                [ReadOnly] ref MoveSpeed c4,
                [ReadOnly] ref AttackRange c5)
    {
        if(c1.Value == States.Attack)
        {
            if(math.distancesq(c2.Value, c3.TargetPos) >= c5.Value)
                c2.Value += math.normalize(c3.TargetPos - c2.Value);
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
