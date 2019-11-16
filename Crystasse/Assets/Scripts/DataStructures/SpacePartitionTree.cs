using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class SpacePartitionTree<T> : Tree<SpacePartition<T>>
{
    public SpacePartitionTree(PartitionType type, Area area)
    {
        var partition = new SpacePartition<T>(type, Constants.MAX_UNIT_PER_PARTITION, area);
        Root = new Node<SpacePartition<T>>(partition);
    }

    public void Add(T value, float3 position)
    {
        Add(value, position, Root);
    }

    protected void Add(T value, float3 position, Node<SpacePartition<T>> node)
    {
        if(node.ConnectionsOut.Count > 0)
        {
            foreach(var currentNode in node.ConnectionsOut)
            {
                if(currentNode.Value.Area.InArea(position))
                    Add(value, position, currentNode);
            }
        }
        else if(!node.Value.IsFull)
        {
            node.Value.Add(value);
        }
        else
        {
            var parts = node.Value.Partition();

            foreach(var partition in parts)
                node.ConnectionsOut.Add(new Node<SpacePartition<T>>(partition));

            //foreach(var conn in node.ConnectionsOut)
            //{
            //    for(int i = 0; i < conn.Value.Values.Length; i++)
            //    {
            //        Add(conn.Value.Values[i], )
            //    }
            //}
        }
    }
}
