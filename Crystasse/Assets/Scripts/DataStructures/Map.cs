using System.Collections;
using System.Collections.Generic;
using Unity.Entities;

public class Map
{
    private static Map _instance = null;
    private Tree<SpacePartition<Entity>> _entityPartition;

    public static Map Instance => _instance ?? (_instance = new Map(new Area(new Unity.Mathematics.float2(),
                                                                             new Unity.Mathematics.float2())));
    public Area Constraints { get; private set; }
    public Node<SpacePartition<Entity>> EntityPartitionRoot => _entityPartition.Root;

    public Map(Area constraints)
    {
        Constraints = constraints;

        var partition = new SpacePartition<Entity>(PartitionType.Quad, Constants.MAX_UNIT_PER_PARTITION, Constraints);
        _entityPartition = new Tree<SpacePartition<Entity>>(new Node<SpacePartition<Entity>>(partition));
    }
}
