using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

public struct UpdateUnitJob : IJobParallelFor
{
    //public NativeArray<Unit> Units;

    public void Execute(int index)
    {
        //Units[index].UpdateUnit();
    }
}
