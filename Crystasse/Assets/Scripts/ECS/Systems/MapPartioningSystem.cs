//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Transforms;

//public class MapPartioningSystem : JobComponentSystem
//{
//    protected int _partitionIndex = 0;
//    protected EntityQuery query;

//    protected override void OnCreate()
//    {
//        base.OnCreate();
//        query = new EntityQueryBuilder().WithAll<LocalToWorld>().ToEntityQuery();
//    }
//    protected override void OnStartRunning()
//    {
//        base.OnStartRunning();
//        MapPartitioningJob.Entities = query.ToEntityArray(Allocator.Temp);
//        MapPartitioningJob.Positions = query.ToComponentDataArray<LocalToWorld>(Allocator.Temp);
//    }
//    protected override JobHandle OnUpdate(JobHandle inputDeps)
//    {
//        var baseRoot = Map.Instance.EntityPartitionRoot;
//        JobHandle job;
//        if(_partitionIndex < baseRoot.ConnectionsOut.Count)
//        {
//            job = new MapPartitioningJob() { Root = baseRoot.ConnectionsOut[_partitionIndex] }.Schedule(
//                baseRoot.ConnectionsOut.Count * MapPartitioningJob.Entities.Length,
//                MapPartitioningJob.Entities.Length,
//                inputDeps);
//            _partitionIndex++;
//            return job;
//        }
//        return inputDeps;
//    }
//}
