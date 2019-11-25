using Unity.Jobs;
using UnityEngine.Jobs;

public struct MoveStatesJob : IJobParallelFor
{
    public void Execute(int index)
    {
        StateMachine.MoveStates[index].UpdateState();
    }
}
