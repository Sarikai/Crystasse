using Unity.Entities;

public class StateMachine : ComponentSystem
{

    protected override void OnUpdate()
    {
        //TODO: Create and schedule jobs.
    }

    public void SwitchState(Entity entity, UnitCommand cmd)
    {
        if(EntityManager.Exists(entity))
        {
            var data = EntityManager.GetComponentData<StateData>(entity);
            data.Command = cmd;
        }
    }
}
