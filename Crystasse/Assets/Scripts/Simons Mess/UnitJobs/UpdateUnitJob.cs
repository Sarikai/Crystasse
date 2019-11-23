using Unity.Jobs;
using UnityEngine.Jobs;

public struct UpdateUnitJob : IJobParallelFor
{
    public Unit[] Units;

    public void Execute(int index)
    {
        Units[index].UpdateUnit();
    }
}
