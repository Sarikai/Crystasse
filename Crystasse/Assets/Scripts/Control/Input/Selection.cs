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

    public static byte TeamID { get; set; }
    public static int PlaneLayer => _data.PlaneLayer;
    public static int SelectionLayer => _data.SelectionLayer;

    public static Unit[] Selected => _selected.ToArray();

    static Selection()
    {
        //TODO: Set owner team ID
        //TeamID = 1;
        TeamID = GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID;
        //Debug.Log($"Selection Team: {TeamID}");

        var text = File.ReadAllText(Constants.SELECTIONDATA_PATH + "/Data.json");
        _data = JsonConvert.DeserializeObject<SelectionData>(text);

        //_sphereVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //_sphereVisual.name = "Selection Sphere Visual";
        //_sphereVisual.GetComponent<SphereCollider>().radius = _data.SelectionRadius;
        //_sphereVisual.GetComponent<SphereCollider>().isTrigger = true;
        //_sphereVisual.SetActive(false);
    }

    private static void AddSelection(Unit[] selection)
    {
        if (selection != null && selection.Length >= 1)
        {
            foreach (var u in selection)
            {
                Debug.Log(u);
            }
            _selected.AddRange(selection);
        }
        else if (selection.Length <= 0)
            _selected.Clear();
    }

    public static void CastSphereSelection(RaycastHit hit)
    {
        //_sphereVisual.transform.position = hit.point;
        //_sphereVisual.SetActive(true);

        var hits = Physics.OverlapSphere(hit.point, _data.SelectionRadius, _data.SelectionLayer);

        List<Unit> sel = new List<Unit>();
        foreach (var coll in hits)
            if (coll.GetComponent<Unit>()?.TeamID == TeamID)
            {
                sel.Add(coll.GetComponent<Unit>());
            }

        _selected.Clear();
        AddSelection(sel.ToArray());
    }

    //TODO: this 
    //public static void CastBoxSelection(Vector3 pos1, Vector3 pos2)
    //{
    //    var halfExtents = (pos1 - pos2) * 0.5f;
    //    var center = pos1 + halfExtents;

    //    var hits = Physics.OverlapBox(center, halfExtents, Quaternion.identity, _data.SelectionLayer);

    //    List<Unit> sel = new List<Unit>();
    //    foreach(var coll in hits)
    //        if(coll.GetComponent<Unit>()?.TeamID == TeamID)
    //        {
    //            sel.Add(coll.GetComponent<Unit>());
    //        }

    //    //_selected.Clear();
    //    AddSelection(sel.ToArray());
    //}
}
