using UnityEngine;

public static class Constants
{
    //TODO: Add correct values
    public const float MAX_UNIT_DISPLACEMENT = 10f;
    public const int MAX_UNIT_PER_PARTITION = 200;
    public const int UNITCOUNT_FOR_UPGRADE = 10;

    private static readonly string DATA_PATH = Application.dataPath + "/DATA";
    public static readonly string CRYSTALDATA_PATH = DATA_PATH + "/Crystal";
    public static readonly string SELECTIONDATA_PATH = DATA_PATH + "/Selection";
}