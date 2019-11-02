using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;


[BurstCompile]
public static class StateMachine
{
    public static void SwitchState(Entity entity, States cmd)
    {
        if(World.Active.EntityManager.Exists(entity))
        {
            var data = World.Active.EntityManager.GetComponentData<State>(entity);
            data.Value = cmd;
        }
    }
}
