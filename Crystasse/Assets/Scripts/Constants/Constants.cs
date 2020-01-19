using UnityEngine;

public static class Constants
{
    public const float MAX_UNIT_DISPLACEMENT = 10f;
    public const int MAX_UNIT_PER_PARTITION = 200;
    public const int UNITCOUNT_FOR_UPGRADE = 10;

    private static readonly string DATA_PATH = Application.dataPath + "/DATA";
    public static readonly string CRYSTALDATA_PATH = DATA_PATH + "/Crystal";
    public static readonly string SELECTIONDATA_PATH = DATA_PATH + "/Selection";
    /// <summary>
    /// Use the TeamID to geht the basic unit prefab path
    /// </summary>
    public static readonly string[] BASIC_UNIT_PREFAB_PATHS = new string[5]
    {
        null,
        //"Gameplay Prefabs/Unit Team 1",
        //"Gameplay Prefabs/Unit Team 2"
       "Gameplay Prefabs/Unit Definitive Edition 01",
       "Gameplay Prefabs/Unit Definitive Edition 02",
       "Gameplay Prefabs/Unit Definitive Edition 03",
       "Gameplay Prefabs/Unit Definitive Edition 04",
    };

    public static readonly string[] NETWORKED_UI_ELEMENTS = new string[1]
    {
        "UI Prefabs/PlayerContentLine"
    };
}