using UnityEngine;

[System.Serializable]
public struct Area
{
    public Vector2 Min;
    public Vector2 Max;

    public Area(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }

    public bool InArea(Vector3 point)
    {
        return InArea(point);
    }

    public bool InArea(Vector2 point)
    {
        bool aboveMin = point.x >= Min.x && point.y >= Min.y;
        bool belowMax = point.x <= Min.x && point.y <= Min.y;

        return aboveMin && belowMax;
    }
}
