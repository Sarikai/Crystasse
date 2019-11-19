using System.Collections;
using System.Collections.Generic;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;

[UnityEngine.CreateAssetMenu(), BurstCompile, System.Serializable]
public class UnitData : UnityEngine.ScriptableObject
{
    public static EntityArchetype Archetype
    {
        get
        {
            return World.Active.EntityManager.CreateArchetype(
                typeof(ID),
                typeof(TeamID),

                typeof(Translation),
                typeof(Rotation),
                typeof(Scale),
                typeof(LocalToWorld),

                typeof(AttackPoints),
                typeof(BuildPoints),
                typeof(HealthPoints),

                typeof(BuildSpeed),

                typeof(Range),

                typeof(TargetPos),
                typeof(RenderMesh),
                typeof(IdleData),
                typeof(MoveData),
                typeof(PhysicsCollider)
                //typeof(PhysicsVelocity),
                );
        }
    }

    public static IdleData DefaultIdleData => new IdleData()
    {
        YDirection = 1f
    };
    public static AttackData DefaultAttackData => new AttackData(-1);
    public static ConquerData DefaultConquerData => new ConquerData();
    public static BuildData DefaultBuildData => new BuildData();

    public ID ID;

    public TeamID teamID;

    public Translation translation;

    public Scale scale;

    public AttackPoints attackPoints;
    public Range range;

    public BuildPoints buildPoints;

    public HealthPoints healthPoints;

    public BuildSpeed buildSpeed;

    public MoveData moveData;

    public TargetPos target;

    public RenderMesh renderMesh;

    public SphereGeometry Geometry => new SphereGeometry()
    {
        Center = translation.Value,
        Radius = range.Value
    };

    public PhysicsCollider Collider => new PhysicsCollider()
    {
        Value = SphereCollider.Create(Geometry)
    };
}
