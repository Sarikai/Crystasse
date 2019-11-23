using Unity.Mathematics;

public struct Area
{
    public float2 Min;
    public float2 Max;

    public Area(float2 min, float2 max)
    {
        Min = min;
        Max = max;
    }

    public Area RightHalf => new Area(Quadrant4.Min, Max);
    public Area LeftHalf => new Area(Quadrant3.Min, Quadrant2.Max);
    public Area Quadrant1 => new Area(Max * 0.5f, Max);
    public Area Quadrant2 => new Area(new float2(Min.x, Max.y * 0.5f),
                                      new float2(Max.x * 0.5f, Max.y));
    public Area Quadrant3 => new Area(Min, Max * 0.5f);
    public Area Quadrant4 => new Area(new float2(Max.x * 0.5f, Min.y),
                                      new float2(Max.x, Max.y * 0.5f));

    public bool InArea(float3 point)
    {
        var cond = new float2(point.x, point.y) >= Min;
        bool aboveMin = cond.x && cond.y;

        cond = new float2(point.x, point.y) <= Max;
        bool belowMax = cond.x && cond.y;

        return aboveMin && belowMax;
    }
}
