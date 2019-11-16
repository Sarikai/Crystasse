using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Physics;
using Unity.Entities;

[BurstCompile]
public static class TransitionRules
{
    private static PhysicsWorld _physicsWorld;
    private static CollisionWorld _collisionWorld;

    static TransitionRules()
    {
        _physicsWorld = World.Active.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>().PhysicsWorld;
        _collisionWorld = _physicsWorld.CollisionWorld;
    }

    public static bool ShouldTransition([ReadOnly] float sqrRange, [ReadOnly] LocalToWorld localToWorld, [ReadOnly] float3 targetPos) => false;
    //sqrRange > 0f && math.distancesq(localToWorld.Position, targetPos) <= sqrRange;

    public static bool ShouldTransition([ReadOnly] LocalToWorld localToWorld, [ReadOnly] float range, [ReadOnly] float3 targetPos) => false;
    //ShouldTransition(math.mul(range, range), localToWorld, targetPos);

    public unsafe static (bool shouldTransition, Entity entityHit) TransitionToAttack([ReadOnly] LocalToWorld localToWorld, [ReadOnly] PhysicsCollider collider)
    {
        ColliderCastInput input = new ColliderCastInput()
        {
            Collider = collider.ColliderPtr,
            Start = localToWorld.Position,
            End = localToWorld.Position,
            Orientation = quaternion.identity
        };

        return (_collisionWorld.CastCollider(input, out ColliderCastHit hit), _physicsWorld.Bodies[hit.RigidBodyIndex].Entity);
    }
}
