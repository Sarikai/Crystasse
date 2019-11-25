using Unity.Jobs;
using UnityEngine.Jobs;

public struct IdleStatesJob : IJobParallelForTransform
{
    public void Execute(int index, TransformAccess transform)
    {
        StateMachine.IdleStates[index].UpdateState();
    }
}
