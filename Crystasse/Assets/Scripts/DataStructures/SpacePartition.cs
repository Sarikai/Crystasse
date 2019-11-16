using System;

public class SpacePartition<T>
{
    public T[] Values { get; set; }
    private int addIndex = 0;

    public T this[int index]
    {
        get
        {
            return Values[index];
        }
        set
        {
            Values[index] = value;
        }
    }
    public PartitionType Type { get; private set; }
    public int MaxAmount { get; private set; }
    public Area Area { get; private set; }
    public bool IsFull => addIndex >= MaxAmount;

    public SpacePartition(PartitionType type, int maxAmount)
    {
        Type = type;
        MaxAmount = maxAmount;
        Values = new T[MaxAmount];
    }

    public SpacePartition(PartitionType type, int maxAmount, Area area) : this(type, maxAmount)
    {
        Type = type;
        MaxAmount = maxAmount;
        Values = new T[MaxAmount];
        Area = area;
    }

    public void Add(T value, int index)
    {
        if(index == -1)
            index = addIndex;
        if(index < MaxAmount)
        {
            Values[index] = value;
            addIndex++;
        }
    }

    public void Add(T value)
    {
        Add(value, -1);
    }

    public SpacePartition<T>[] Partition()
    {
        var areas = Array.Empty<Area>();
        switch(Type)
        {
            case PartitionType.Binary:
                areas = new Area[2] { Area.LeftHalf, Area.RightHalf };
                break;
            case PartitionType.Quad:
                areas = new Area[4] { Area.Quadrant1, Area.Quadrant2, Area.Quadrant3, Area.Quadrant4 };
                break;
        }

        var result = new SpacePartition<T>[areas.Length];

        for(int i = 0; i < result.Length; i++)
            result[i] = new SpacePartition<T>(Type, MaxAmount, result[i].Area);

        return result;
    }

    ~SpacePartition()
    {
        if(typeof(T) == typeof(IDisposable))
            for(int i = 0; i < Values.Length; i++)
                (Values[i] as IDisposable).Dispose();
    }
}
