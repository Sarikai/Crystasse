using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Physics;

public static class EntitySpawnHelper
{
    public static Entity SpawnEntity(EntityArchetype archetype) => World.Active.EntityManager.CreateEntity(archetype);

    public static Entity SpawnEntity(EntityArchetype archetype, EntityManager manager) => manager.CreateEntity(archetype);

    public static Entity SpawnEntityWithValues(EntityArchetype archetype, EntityManager manager, UnitData data)
    {
        var e = manager.CreateEntity(archetype);

        AssignDefaultValues(e, manager, data);

        return e;
    }

    public static Entity SpawnEntityWithValues(EntityArchetype archetype, EntityManager manager, CrystalEntityData data)
    {
        var e = manager.CreateEntity(archetype);

        AssignDefaultValues(e, manager, data);

        return e;
    }

    private static void AssignDefaultValues(Entity e, EntityManager manager, CrystalEntityData data)
    {
        manager.SetSharedComponentData<TeamID>(e, data.teamID);

        manager.SetComponentData<CrystalID>(e, data.ID);
        manager.SetComponentData<Translation>(e, data.translation);
        manager.SetComponentData<Range>(e, data.range);
        manager.SetComponentData<PhysicsCollider>(e, data.Collider);
    }

    private static void AssignDefaultValues(Entity e, EntityManager manager, UnitData data)
    {
        manager.SetSharedComponentData<TeamID>(e, data.teamID);
        manager.SetSharedComponentData<RenderMesh>(e, data.renderMesh);

        manager.SetComponentData<ID>(e, data.ID);
        manager.SetComponentData<Translation>(e, data.translation);
        manager.SetComponentData<Scale>(e, data.scale);
        manager.SetComponentData<AttackPoints>(e, data.attackPoints);
        manager.SetComponentData<Range>(e, data.range);
        manager.SetComponentData<BuildPoints>(e, data.buildPoints);
        manager.SetComponentData<HealthPoints>(e, data.healthPoints);
        manager.SetComponentData<BuildSpeed>(e, data.buildSpeed);
        manager.SetComponentData<MoveData>(e, data.moveData);
        manager.SetComponentData<TargetPos>(e, data.target);
        //TODO: IdleData
        manager.SetComponentData<IdleData>(e, UnitData.DefaultIdleData);
        manager.SetComponentData<PhysicsCollider>(e, data.Collider);
    }
}
