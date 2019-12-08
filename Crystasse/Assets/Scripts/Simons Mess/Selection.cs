using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class Selection
{
    private static readonly string _path = $"{Application.dataPath}/Data/Selection/Data.json";

    private static readonly List<Unit> _selected = new List<Unit>();
    private static SelectionData _data;
    private static readonly GameObject _sphereVisual;

    public static int PlaneLayer => _data.PlaneLayer;
    public static int SelectionLayer => _data.SelectionLayer;

    public static Unit[] Selected => _selected.ToArray();

    static Selection()
    {
        var text = File.ReadAllText(Constants.SELECTIONDATA_PATH + "/Data.json");
        _data = JsonConvert.DeserializeObject<SelectionData>(text);

        _sphereVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _sphereVisual.name = "Selection Sphere Visual";
        _sphereVisual.GetComponent<SphereCollider>().radius =/*transform.localScale = new Vector3(*/_data.SelectionRadius/*, _data.SelectionRadius, _data.SelectionRadius)*/;
        _sphereVisual.GetComponent<SphereCollider>().isTrigger = true;
        _sphereVisual.SetActive(false);
    }

    private static void AddSelection(Unit[] selection)
    {
        if(selection != null && selection.Length >= 1)
            _selected.AddRange(selection);
        else if(selection.Length <= 0)
            _selected.Clear();

        foreach(var sel in _selected)
        {
            Debug.Log(sel.name);
        }
    }

    public static void CastSphereSelection(RaycastHit hit)
    {
        _sphereVisual.transform.position = hit.point;
        _sphereVisual.SetActive(true);

        Debug.Log("Cast OverlapSphere" + _data.SelectionLayer);

        var hits = Physics.OverlapSphere(hit.point, _data.SelectionRadius, _data.SelectionLayer);

        foreach(var hot in hits)
        {
            Debug.Log("Hit: " + hot.name);
        }

        List<Unit> sel = new List<Unit>();
        //TODO: Add TeamID for selection
        foreach(var coll in hits)
            if(coll.GetComponent<Unit>())
                sel.Add(coll.GetComponent<Unit>());

        _selected.Clear();
        AddSelection(sel.ToArray());
    }

    public static void CastBoxSelection(Vector3 pos1, Vector3 pos2)
    {
        Unit[] selected = new Unit[0];

        AddSelection(selected);
    }
}
