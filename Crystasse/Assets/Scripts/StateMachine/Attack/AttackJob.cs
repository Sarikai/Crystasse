using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;

//[BurstCompile]
unsafe struct AttackJob : IJobForEachWithEntity<AttackPoints, PhysicsCollider, AttackData, MoveData, Range>
{
    LocalToWorld _localToWorld;

    public void Execute(Entity entity,
                int index,
                [ReadOnly] ref AttackPoints _attackPoints,
                ref PhysicsCollider collider,
                ref AttackData _data,
                [ReadOnly] ref MoveData _moveData,
                [ReadOnly] ref Range _range)
    {
        _localToWorld = World.Active.EntityManager.GetComponentData<LocalToWorld>(entity);

        ColliderCastInput castInput = CreateCastInput(ref collider);

        if(HasAttacked(AttackSystem.world.EntityManager, castInput, entity, _data.TargetID, _attackPoints.Value))
            return;

        if(_data.TargetID > -1)
            _moveData.Direction = math.normalize(_data.TargetPos - _localToWorld.Position);
    }

    private ColliderCastInput CreateCastInput(ref PhysicsCollider collider)
    {
        return new ColliderCastInput()
        {
            Collider = collider.ColliderPtr,
            Start = _localToWorld.Position,
            End = _localToWorld.Position,
            Orientation = quaternion.identity
        };
    }

    private void TakeDamage(EntityManager entityManager, Entity e, HealthPoints hP, byte aP)
    {
        hP.Value -= aP;
        entityManager.SetComponentData<HealthPoints>(e, hP);
    }

    private bool HasAttacked(EntityManager entityManager, ColliderCastInput input, Entity entity, int targetID, byte attackPoints)
    {
        var allHits = new NativeList<ColliderCastHit>();
        var physicsWorld = AttackSystem.world.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>().PhysicsWorld;
        if(physicsWorld.CollisionWorld.CastCollider(input, ref allHits))
        {
            if(targetID > -1)
                return AttackTarget(entityManager, physicsWorld, entity, targetID, attackPoints, allHits);
            else
            {
                AttackEntity(entityManager, entity, physicsWorld.Bodies[0].Entity, attackPoints);
                return true;
            }
        }
        return false;
    }

    private bool AttackTarget(EntityManager entityManager, PhysicsWorld physicsWorld, Entity entity, int targetID, byte attackPoints, NativeList<ColliderCastHit> allHits)
    {
        foreach(var hit in allHits)
        {
            var enemyHit = physicsWorld.Bodies[hit.RigidBodyIndex].Entity;

            if(entityManager.GetComponentData<ID>(enemyHit).Value == targetID)
            {
                AttackEntity(entityManager, entity, enemyHit, attackPoints);
                return true;
            }
        }
        return false;
    }

    private void AttackEntity(EntityManager entityManager, Entity entity, Entity enemy, byte attackPoints)
    {
        var aP = entityManager.GetComponentData<AttackPoints>(enemy);
        var hP = entityManager.GetComponentData<HealthPoints>(enemy);

        TakeDamage(entityManager, enemy, hP, attackPoints);
        TakeDamage(entityManager, entity, entityManager.GetComponentData<HealthPoints>(entity), aP.Value);
    }
}
