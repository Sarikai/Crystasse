using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Selection
{
    private static readonly string _path = $"{Application.dataPath}/Data/Selection/Data.json";

    private static List<Unit> _selected = new List<Unit>();
    private static SelectionData _data;

    public static Unit[] Selected => _selected.ToArray();

    static Selection()
    {
        _data = JsonUtility.FromJson<SelectionData>(File.ReadAllText(_path));
    }

    private static void AddSelection(Unit[] selection)
    {
        if(selection != null && selection.Length >= 1)
            _selected.AddRange(selection);
        else if(selection.Length <= 0)
            _selected.Clear();
    }

    public static void CastSphereSelection(RaycastHit hit)
    {
        //TODO: Query for all entities of the Archetype for the units and decide which of them are in range.
        Unit[] selected = new Unit[0];

        AddSelection(selected);
    }

    public static void CastBoxSelection(Vector3 pos1, Vector3 pos2)
    {
        //TODO: Query for all entities of the Archetype for the units and decide which of them are inside the box.
        Unit[] selected = new Unit[0];

        AddSelection(selected);
    }
}
