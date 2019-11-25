using Unity.Jobs;
using UnityEngine.Jobs;

public struct BuildStatesJob : IJobParallelFor
{
    public void Execute(int index)
    {
        StateMachine.BuildStates[index].UpdateState();
    }
}
