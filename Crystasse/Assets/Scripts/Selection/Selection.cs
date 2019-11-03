using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using UnityEngine;

public static class Selection
{
    private static readonly string _path = $"{Application.dataPath}/Data/Selection/Data.json";

    private static List<Entity> _selected = new List<Entity>();
    private static SelectionData _data;

    public static Entity[] Selected => _selected.ToArray();

    static Selection()
    {
        _data = JsonUtility.FromJson<SelectionData>(File.ReadAllText(_path));
    }

    private static void AddSelection(Entity[] selection)
    {
        if(selection != null && selection.Length >= 1)
            _selected.AddRange(selection);
        else if(selection.Length <= 0)
            _selected.Clear();
    }

    public static void CastSphereSelection(RaycastHit hit)
    {
        //TODO: Query for all entities of the Archetype for the units and decide which of them are in range.
        Entity[] selected = new Entity[0];

        AddSelection(selected);
    }

    public static void CastBoxSelection(Vector3 pos1, Vector3 pos2)
    {
        //TODO: Query for all entities of the Archetype for the units and decide which of them are inside the box.
        Entity[] selected = new Entity[0];

        AddSelection(selected);
    }
}
