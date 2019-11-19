using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[UnityEngine.CreateAssetMenu(), BurstCompile, System.Serializable]
public class CrystalEntityData : UnityEngine.ScriptableObject
{
    public static EntityArchetype Archetype
    {
        get
        {
            return World.Active.EntityManager.CreateArchetype(
                typeof(CrystalID),
                typeof(TeamID),
                typeof(Translation),
                typeof(Rotation),
                typeof(LocalToWorld),
                typeof(Range),
                typeof(PhysicsCollider),
                typeof(PhysicsVelocity)
                );
        }
    }
    public CrystalID ID;
    public TeamID teamID;
    public Translation translation;
    public Range range;

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