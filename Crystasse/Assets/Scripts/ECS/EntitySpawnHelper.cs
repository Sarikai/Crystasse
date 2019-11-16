using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;

public static class EntitySpawnHelper
{
    public static Entity SpawnEntity(EntityArchetype archetype) => World.Active.EntityManager.CreateEntity(archetype);

    public static Entity SpawnEntity(EntityArchetype archetype, World world) => world.EntityManager.CreateEntity(archetype);

    public static Entity SpawnEntityWithValues(EntityArchetype archetype, World world, UnitData data)
    {
        var e = world.EntityManager.CreateEntity(archetype);

        AssignDefaultValues(e, world, data);

        return e;
    }

    private static void AssignDefaultValues(Entity e, World world, UnitData data)
    {
        world.EntityManager.SetSharedComponentData<TeamID>(e, data.teamID);
        world.EntityManager.SetSharedComponentData<RenderMesh>(e, data.renderMesh);

        world.EntityManager.SetComponentData<ID>(e, data.ID);
        world.EntityManager.SetComponentData<Translation>(e, data.translation);
        world.EntityManager.SetComponentData<Scale>(e, data.scale);
        world.EntityManager.SetComponentData<AttackPoints>(e, data.attackPoints);
        world.EntityManager.SetComponentData<Range>(e, data.range);
        world.EntityManager.SetComponentData<BuildPoints>(e, data.buildPoints);
        world.EntityManager.SetComponentData<HealthPoints>(e, data.healthPoints);
        world.EntityManager.SetComponentData<BuildSpeed>(e, data.buildSpeed);
        world.EntityManager.SetComponentData<MoveSpeed>(e, data.moveSpeed);
        world.EntityManager.SetComponentData<TargetPos>(e, data.target);
        //TODO: IdleData
        world.EntityManager.SetComponentData<AttackData>(e, UnitData.DefaultAttackData);
    }
}
