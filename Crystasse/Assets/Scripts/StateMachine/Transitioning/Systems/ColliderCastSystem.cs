using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public class ColliderCastSystem : ComponentSystem
{
    PhysicsWorld _physicsWorld;

    NativeArray<PhysicsCollider> _crystals;
    NativeArray<PhysicsCollider> _bridges;

    //TODO: Create Filters
    CollisionFilter crystalFilter;
    CollisionFilter bridgeFilter;

    protected override void OnCreate()
    {
        base.OnCreate();

        _crystals = GetEntityQuery(typeof(CrystalID), typeof(PhysicsCollider)).ToComponentDataArray<PhysicsCollider>(Allocator.Persistent);
        _bridges = GetEntityQuery(typeof(BridgeID), typeof(PhysicsCollider)).ToComponentDataArray<PhysicsCollider>(Allocator.Persistent);

        _physicsWorld = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>().PhysicsWorld;
    }

    protected override void OnUpdate()
    {
        foreach(var collider in _crystals)
            SwitchNearbyEntities(collider, States.Conquer, crystalFilter);

        foreach(var collider in _bridges)
            SwitchNearbyEntities(collider, States.Build, bridgeFilter);
    }

    protected unsafe void SwitchNearbyEntities(PhysicsCollider collider, States cmd, CollisionFilter filter)
    {
        var input = new ColliderCastInput()
        {
            Collider = collider.ColliderPtr,
            Orientation = quaternion.identity
        };

        var prevFilter = collider.Value.Value.Filter;

        collider.Value.Value.Filter = filter;

        NativeList<ColliderCastHit> allHits = new NativeList<ColliderCastHit>();
        if(_physicsWorld.CollisionWorld.CastCollider(input, ref allHits))
            foreach(var hit in allHits)
                TransitionSystem.SwitchState(_physicsWorld.Bodies[hit.RigidBodyIndex].Entity, cmd);

        collider.Value.Value.Filter = prevFilter;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _crystals.Dispose();
        _bridges.Dispose();
    }
}
