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
        "UI_Prefabs/PlayerContentLine"
    };

    public static readonly string[] UNIT_ICONS = new string[20]
    {
        "UI_Icons/Unit01",
        "UI_Icons/Unit02",
        "UI_Icons/Unit03",
        "UI_Icons/Unit04",
        "UI_Icons/Unit05",
        "UI_Icons/Unit06",
        "UI_Icons/Unit07",
        "UI_Icons/Unit08",
        "UI_Icons/Unit09",
        "UI_Icons/Unit10",
        "UI_Icons/Unit11",
        "UI_Icons/Unit12",
        "UI_Icons/Unit13",
        "UI_Icons/Unit14",
        "UI_Icons/Unit15",
        "UI_Icons/Unit16",
        "UI_Icons/Unit17",
        "UI_Icons/Unit18",
        "UI_Icons/Unit19",
        "UI_Icons/Unit20",
    };

}