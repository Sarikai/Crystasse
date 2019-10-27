using Unity.Entities;
using Unity.Jobs;

public class StateMachine : ComponentSystem
{
    private IdleSystem _idleSystem;
    private AttackSystem _attackSystem;
    private ConquerSystem _conquerSystem;
    private BuildSystem _buildSystem;
    private TransitionSystem _transitionSystem;

    protected override void OnUpdate()
    {
        GetSystemReferencesIfNull();

        _idleSystem.Update();
        _attackSystem.Update();
        _conquerSystem.Update();
        _buildSystem.Update();

        _transitionSystem.Update();

        JobHandle.ScheduleBatchedJobs();
    }

    public void SwitchState(Entity entity, States cmd)
    {
        if(EntityManager.Exists(entity))
        {
            var data = EntityManager.GetComponentData<State>(entity);
            data.Value = cmd;
        }
    }

    private void GetSystemReferencesIfNull()
    {
        if(_idleSystem == null)
            _idleSystem = World.Active.GetOrCreateSystem<IdleSystem>();
        if(_attackSystem == null)
            _attackSystem = World.Active.GetOrCreateSystem<AttackSystem>();
        if(_conquerSystem == null)
            _conquerSystem = World.Active.GetOrCreateSystem<ConquerSystem>();
        if(_buildSystem == null)
            _buildSystem = World.Active.GetOrCreateSystem<BuildSystem>();
        if(_transitionSystem == null)
            _transitionSystem = World.Active.GetOrCreateSystem<TransitionSystem>();
    }
}
