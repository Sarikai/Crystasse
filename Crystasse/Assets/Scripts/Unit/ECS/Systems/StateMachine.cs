using Unity.Entities;

public class StateMachine : ComponentSystem
{
    protected override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void SwitchState(Entity entity, UnitCommand cmd)
    {
        if(EntityManager.Exists(entity))
        {
            var data = EntityManager.GetSharedComponentData<StateData>(entity);
            data.Command = cmd;
        }
    }
}
