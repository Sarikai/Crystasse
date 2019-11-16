//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Transforms;

//public struct MapPartitioningJob : IJobParallelFor
//{
//    [ReadOnly]
//    public static NativeArray<LocalToWorld> Positions;
//    [ReadOnly]
//    public static NativeArray<Entity> Entities;
//    public Node<SpacePartition<Entity>> Root;

//    public void Execute(int index)
//    {
//        index %= Entities.Length;
//        if(InArea(index))
//        {
//            if(!Root.Value.IsFull)
//            {
//                Root.Value.Add(Entities[index]);
//            }
//            else
//            {

//            }
//        }
//    }

//    private bool InArea(int index)
//    {
//        var currentPos = Positions[index].Position;

//        var cond = new float2(currentPos.x, currentPos.y) >= Root.Value.Area.Min;
//        bool aboveMin = cond.x && cond.y;

//        cond = new float2(currentPos.x, currentPos.y) <= Root.Value.Area.Max;
//        bool belowMax = cond.x && cond.y;

//        return aboveMin && belowMax;
//    }
//}
