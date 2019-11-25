using Unity.Jobs;
using UnityEngine.Jobs;

public struct AttackStatesJob : IJobParallelFor
{
    public void Execute(int index)
    {
        StateMachine.AttackStates[index].UpdateState();
    }
}
