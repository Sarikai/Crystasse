using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Selection
{
    private static readonly string _path = $"{Application.dataPath}/Data/Selection/Data.json";

    private static List<Unit> _selected;
    private static SelectionData _data;

    public static Unit[] Selected => _selected.ToArray();

    static Selection()
    {
        _data = JsonUtility.FromJson<SelectionData>(File.ReadAllText(_path));
    }

    private static void AddSelection(Collider[] selection)
    {
        if(selection != null && selection.Length >= 1)
            foreach(var collider in selection)
            {
                //TODO: Make ECS Compatible
                var unit = collider.GetComponent<Unit>();

                if(unit)
                    _selected.Add(unit);
            }
        else if(selection.Length <= 0)
            _selected.Clear();
    }

    public static void CastSphereSelection(RaycastHit hit)
    {
        var selected = Physics.OverlapSphere(hit.point, _data.SelectionRadius, _data.SelectionLayer);

        AddSelection(selected);
    }

    public static void CastBoxSelection(Vector3 pos1, Vector3 pos2)
    {
        var (center, halfExtents) = CalculateBoxParameter(pos1, pos2);
        var selected = Physics.OverlapBox(center, halfExtents, Quaternion.identity, _data.SelectionLayer);

        AddSelection(selected);
    }

    public static (Vector3 center, Vector3 extents) CalculateBoxParameter(Vector3 pos1, Vector3 pos2)
    {
        var vec = (pos2 - pos1) * 0.5f;

        return (pos1 + vec, new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z)));
    }
}
