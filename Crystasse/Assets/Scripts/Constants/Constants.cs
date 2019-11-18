using Unity.Physics;
using UnityEngine;

public static class Constants
{
    //TODO: Add correct values
    public const float MAX_UNIT_DISPLACEMENT = 15f;
    public const int MAX_UNIT_PER_PARTITION = 200;

    private static readonly string DATA_PATH = Application.dataPath + "/DATA";
    public static readonly string CRYSTALDATA_PATH = DATA_PATH + "/Crystal";
    public static readonly string SELECTIONDATA_PATH = DATA_PATH + "/Selection";

    public static readonly CollisionFilter AttackFilter = new CollisionFilter()
    {
        BelongsTo = (uint)1 << 31,
        CollidesWith = (uint)1 << 31 &
                       BuildFilter.BelongsTo &
                       ConquerFilter.BelongsTo &
                       IdleFilter.BelongsTo &
                       MoveFilter.BelongsTo
    };
    public static readonly CollisionFilter BuildFilter = new CollisionFilter()
    {
        BelongsTo = (uint)1 << 30,
        CollidesWith = 0
    };
    public static readonly CollisionFilter ConquerFilter = new CollisionFilter()
    {
        BelongsTo = (uint)1 << 29,
        CollidesWith = 0
    };
    public static readonly CollisionFilter IdleFilter = new CollisionFilter()
    {
        BelongsTo = (uint)1 << 28,
        CollidesWith = 0
    };
    public static readonly CollisionFilter MoveFilter = new CollisionFilter()
    {
        BelongsTo = (uint)1 << 27,
        CollidesWith = 0
    };
    public static readonly CollisionFilter StaticEntityFilter = new CollisionFilter()
    {
        BelongsTo = (uint)1 << 26,
        CollidesWith = IdleFilter.BelongsTo &
                       MoveFilter.BelongsTo
    };

    public static CollisionFilter GetStateFilter(byte teamID, States state)
    {
        CollisionFilter filter;
        switch(state)
        {
            case States.Idle:
                filter = IdleFilter;
                break;
            case States.Build:
                filter = BuildFilter;
                break;
            case States.Attack:
                filter = AttackFilter;
                break;
            case States.Conquer:
                filter = ConquerFilter;
                break;
            default:
                filter = IdleFilter;
                break;
        }
        filter.BelongsTo |= teamID;
        byte teamLayer = (byte)(255 & ~teamID);
        filter.CollidesWith |= teamLayer;

        return filter;
    }
}