using System.Collections;
using System.Collections.Generic;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;

[CreateAssetMenu(), BurstCompile]
public class UnitData : ScriptableObject
{
    public static EntityArchetype Archetype
    {
        get
        {
            return World.Active.EntityManager.CreateArchetype(
                typeof(ID),
                typeof(TeamID),
                typeof(Translation),
                typeof(Scale),
                typeof(LocalToWorld),
                typeof(AttackPoints),
                typeof(BuildPoints),
                typeof(HealthPoints),
                typeof(BuildSpeed),
                typeof(MoveSpeed),
                typeof(AttackRange),
                typeof(BuildRange),
                typeof(ConquerRange),
                typeof(State),
                typeof(TargetPos),
                typeof(RenderMesh),
                typeof(IdleData)
                );
        }
    }

    public static IdleData DefaultIdleData
    {
        get
        {
            return new IdleData() { YDirection = 1f };
        }
    }
    public static AttackData DefaultAttackData
    {
        get
        {
            return new AttackData();
        }
    }
    public static ConquerData DefaultConquerData
    {
        get
        {
            return new ConquerData();
        }
    }
    public static BuildData DefaultBuildData
    {
        get
        {
            return new BuildData();
        }
    }

    public ID id;

    public TeamID teamID;

    public Translation t;

    public Scale s;

    public AttackPoints ap;
    public AttackRange aRange;

    public BuildPoints bp;
    public BuildRange bRange;

    public HealthPoints hp;

    public BuildSpeed bs;

    public MoveSpeed ms;

    public State sD;

    public TargetPos target;

    public RenderMesh m;
}
