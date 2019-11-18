using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public struct ColliderCastJob : IJobForEachWithEntity<PhysicsCollider, LocalToWorld>
{
    public static PhysicsWorld PhysicsWorld;

    public unsafe void Execute(Entity entity, int index, ref PhysicsCollider c0, ref LocalToWorld localToWorld)
    {
        var entityManager = World.Active.EntityManager;

        var input = new ColliderCastInput()
        {
            Collider = c0.ColliderPtr,
            Start = localToWorld.Position,
            End = localToWorld.Position,
            Orientation = quaternion.identity
        };

        NativeList<ColliderCastHit> allHits = new NativeList<ColliderCastHit>();
        if(PhysicsWorld.CollisionWorld.CastCollider(input, ref allHits))
        {
            if(entityManager.HasComponent<AttackData>(entity))
            {
                var data = entityManager.GetComponentData<AttackData>(entity);
                data.TargetID = GetID(entityManager, entity);
                entityManager.SetComponentData(entity, data);
            }
            foreach(var hit in allHits)
            {
                var hitEntity = PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                if(entityManager.HasComponent<CrystalID>(hitEntity))
                {
                    TransitionSystem.SwitchState(entity, States.Conquer);
                    return;
                }
                else if(entityManager.HasComponent<BridgeID>(hitEntity))
                {
                    TransitionSystem.SwitchState(entity, States.Build);
                    return;
                }
            }


        }
    }

    private int GetID(EntityManager eManager, Entity e)
    {
        if(eManager.HasComponent<ID>(e))
            return eManager.GetComponentData<ID>(e).Value;
        if(eManager.HasComponent<CrystalID>(e))
            return eManager.GetComponentData<CrystalID>(e).Value;
        if(eManager.HasComponent<BridgeID>(e))
            return eManager.GetComponentData<BridgeID>(e).Value;

        return -1;
    }
}
