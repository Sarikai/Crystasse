using UnityEngine;

public static class Constants
{
    public const float MAX_UNIT_DISPLACEMENT = 10f;
    public const int MAX_UNIT_PER_PARTITION = 200;
    public const int UNITCOUNT_FOR_UPGRADE = 10;

    private static readonly string DATA_PATH = Application.dataPath + "/Data";
    public static readonly string CRYSTALDATA_PATH = DATA_PATH + "/Crystal";
    public static readonly string SELECTIONDATA_PATH = DATA_PATH + "/Selection";

    /// <summary>
    /// Use the TeamID to geht the basic unit prefab path
    /// </summary>
    public static readonly string[] BASIC_UNIT_PREFAB_PATHS = new string[21]
    {
        null,
       "Level_Objects/Unit_Prefabs/Unit 001",
       "Level_Objects/Unit_Prefabs/Unit 002",
       "Level_Objects/Unit_Prefabs/Unit 003",
       "Level_Objects/Unit_Prefabs/Unit 004",
       "Level_Objects/Unit_Prefabs/Unit 005",
       "Level_Objects/Unit_Prefabs/Unit 006",
       "Level_Objects/Unit_Prefabs/Unit 007",
       "Level_Objects/Unit_Prefabs/Unit 008",
       "Level_Objects/Unit_Prefabs/Unit 009",
       "Level_Objects/Unit_Prefabs/Unit 010",
       "Level_Objects/Unit_Prefabs/Unit 011",
       "Level_Objects/Unit_Prefabs/Unit 012",
       "Level_Objects/Unit_Prefabs/Unit 013",
       "Level_Objects/Unit_Prefabs/Unit 014",
       "Level_Objects/Unit_Prefabs/Unit 015",
       "Level_Objects/Unit_Prefabs/Unit 016",
       "Level_Objects/Unit_Prefabs/Unit 017",
       "Level_Objects/Unit_Prefabs/Unit 018",
       "Level_Objects/Unit_Prefabs/Unit 019",
       "Level_Objects/Unit_Prefabs/Unit 020"
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