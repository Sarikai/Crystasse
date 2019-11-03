using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;

public static class EntitySpawnHelper
{
    public static Entity SpawnEntity(EntityArchetype archetype)
    {
        return World.Active.EntityManager.CreateEntity(archetype);
    }

    public static Entity SpawnEntity(EntityArchetype archetype, World world)
    {
        return world.EntityManager.CreateEntity(archetype);
    }

    public static Entity SpawnEntityWithValues(EntityArchetype archetype, World world, UnitData data)
    {
        var e = world.EntityManager.CreateEntity(archetype);

        AssignDefaultValues(e, world, data);

        return e;
    }

    private static void AssignDefaultValues(Entity e, World world, UnitData data)
    {
        world.EntityManager.SetSharedComponentData<TeamID>(e, data.teamID);
        world.EntityManager.SetSharedComponentData<RenderMesh>(e, data.m);

        world.EntityManager.SetComponentData<ID>(e, data.id);
        world.EntityManager.SetComponentData<Translation>(e, data.t);
        world.EntityManager.SetComponentData<Scale>(e, data.s);
        world.EntityManager.SetComponentData<AttackPoints>(e, data.ap);
        world.EntityManager.SetComponentData<BuildPoints>(e, data.bp);
        world.EntityManager.SetComponentData<HealthPoints>(e, data.hp);
        world.EntityManager.SetComponentData<BuildSpeed>(e, data.bs);
        world.EntityManager.SetComponentData<MoveSpeed>(e, data.ms);
        world.EntityManager.SetComponentData<State>(e, data.sD);
        world.EntityManager.SetComponentData<Target>(e, data.target);
        world.EntityManager.SetComponentData<IdleData>(e, UnitData.DefaultIdleData);
    }
}
