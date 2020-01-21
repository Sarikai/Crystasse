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
    const int crystalLayer = 1 << 12;
    const int bridgeLayer = 1 << 13;
    private static readonly GameObject _sphereVisual;

    public static byte TeamID { get; set; }
    public static int PlaneLayer => _data.PlaneLayer;
    public static int SelectionLayer => _data.SelectionLayer;

    public static Unit[] Selected => _selected.ToArray();
    public static bool HasValidSelection => Selected != null && Selected.Length > 0;

    public static int BridgeLayer => bridgeLayer;

    public static int CrystalLayer => crystalLayer;

    static Selection()
    {
        //TODO: [DONE/SHOULD BE] Set owner team ID
        //TeamID = 1;
        TeamID = GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID;
        //Debug.Log($"Selection Team: {TeamID}");

        var text = File.ReadAllText(Constants.SELECTIONDATA_PATH + "/Data.json");
        _data = JsonConvert.DeserializeObject<SelectionData>(text);
    }

    private static void AddSelection(Unit[] selection)
    {
        //foreach(var u in selection)
        //{
        //    Debug.Log(u);
        //}
        if(selection != null && selection.Length >= 1)
            _selected.AddRange(selection);
        else if(selection.Length <= 0)
            _selected.Clear();
    }

    public static void CastSphereSelection(RaycastHit hit)
    {
        var hits = Physics.OverlapSphere(hit.point, _data.SelectionRadius, _data.SelectionLayer);

        List<Unit> sel = new List<Unit>();
        foreach(var coll in hits)
        {
            var unit = coll.GetComponent<Unit>();
            if(unit != null && unit.TeamID == TeamID)
                sel.Add(unit);
        }

        _selected.Clear();
        AddSelection(sel.ToArray());
    }

    //TODO: this 
    public static void CastBoxSelection(Vector3 pos1, Vector3 pos2)
    {
        var halfExtents = (pos1 - pos2) * 0.5f;
        var center = pos1 + halfExtents;

        var hits = Physics.OverlapBox(center, halfExtents, Quaternion.identity, _data.SelectionLayer);

        List<Unit> sel = new List<Unit>();
        foreach(var coll in hits)
        {
            var unit = coll.GetComponent<Unit>();
            if(unit != null && unit.TeamID == TeamID)
                sel.Add(unit);
        }

        //_selected.Clear();
        AddSelection(sel.ToArray());
    }
}
