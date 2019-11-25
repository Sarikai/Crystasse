using Unity.Jobs;
using UnityEngine.Jobs;

public struct ConquerStatesJob : IJobParallelFor
{
    public void Execute(int index)
    {
        StateMachine.ConquerStates[index].UpdateState();
    }
}
