using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;


[BurstCompile]
public class StateMachine : ComponentSystem
{
    private IdleSystem _idleSystem = null;
    private AttackSystem _attackSystem = null;
    private ConquerSystem _conquerSystem = null;
    private BuildSystem _buildSystem = null;
    private TransitionCSystem _transitionCSystem = null;
    private TransitionSystem _transitionSystem = null;

    protected override void OnUpdate()
    {
        GetSystemReferencesIfNull();

        _idleSystem.Update();
        _attackSystem.Update();
        _conquerSystem.Update();
        _buildSystem.Update();
        _transitionSystem.Update();
        JobHandle.ScheduleBatchedJobs();

        _transitionCSystem.Update();
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
        if(_transitionCSystem == null)
            _transitionCSystem = World.Active.GetOrCreateSystem<TransitionCSystem>();
        if(_transitionSystem == null)
            _transitionSystem = World.Active.GetOrCreateSystem<TransitionSystem>();
    }
}
