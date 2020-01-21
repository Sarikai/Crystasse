using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class Selection
{
    private static readonly string _path = $"{Application.dataPath}/Data/Selection/Data.json";

    private static readonly List<Unit> _selected = new List<Unit>();
    private const float _selectionRadius = 10f;
    private const int _planeLayer = 1 << 8;
    private const int _unitLayer = 1 << 11;
    private const int _crystalLayer = 1 << 12;
    private const int _bridgeLayer = 1 << 13;
    private const int _selectionLayer = 1 << 9;
    private static readonly GameObject _sphereVisual;

    public static byte TeamID { get; set; }
    public static int PlaneLayer => _planeLayer;
    public static int UnitLayer => _unitLayer;

    public static Unit[] Selected => _selected.ToArray();
    public static bool HasValidSelection => Selected != null && Selected.Length > 0;

    public static int BridgeLayer => _bridgeLayer;

    public static int CrystalLayer => _crystalLayer;

    public static int SelectionLayer => _selectionLayer;

    static Selection()
    {
        TeamID = GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID;
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
        var hits = Physics.OverlapSphere(hit.point, _selectionRadius, _unitLayer);

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
    public static Crystal[] CastSphereSelectionCrystal(RaycastHit hit)
    {
        var hits = Physics.OverlapSphere(hit.point, _selectionRadius * 0.5f, _crystalLayer);

        List<Crystal> sel = new List<Crystal>();
        foreach(var coll in hits)
        {
            var crystal = coll.GetComponentInChildren<Crystal>();
            if(crystal != null && crystal.TeamID == TeamID)
                sel.Add(crystal);
        }

        return sel.ToArray();
    }

    public static void CastBoxSelection(Vector3 pos1, Vector3 pos2)
    {
        var halfExtents = (pos1 - pos2) * 0.5f;
        var center = pos1 + halfExtents;

        var hits = Physics.OverlapBox(center, halfExtents, Quaternion.identity, _unitLayer);

        List<Unit> sel = new List<Unit>();
        foreach(var coll in hits)
        {
            var unit = coll.GetComponent<Unit>();
            if(unit != null && unit.TeamID == TeamID)
                sel.Add(unit);
        }
        AddSelection(sel.ToArray());
    }
}
