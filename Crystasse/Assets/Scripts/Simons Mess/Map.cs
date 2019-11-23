using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[CreateAssetMenu()]
public class Map : ScriptableObject
{
    [Header("All Crystals"), Tooltip("List of all Crystals.")]
    public List<Crystal> Crystals = new List<Crystal>();

    [Header("Crystal Connections"), Tooltip("IDs of the Crystals the Connection is FROM.")]
    public List<int> CrystalIds = new List<int>();

    [Tooltip("Crystals the Connection goes TO.")]
    public List<CrystalsConnected> ConnectedCrystals = new List<CrystalsConnected>();

    public SortedDictionary<int, List<Crystal>> CrystalConnections { get; private set; }

    private void OnValidate()
    {
        if(CrystalConnections == null)
            CrystalConnections = new SortedDictionary<int, List<Crystal>>();

        int length = CrystalIds.Count <= ConnectedCrystals.Count ? CrystalIds.Count : ConnectedCrystals.Count;

        for(int i = 0; i < length; i++)
            CrystalConnections.Add(CrystalIds[i], ConnectedCrystals[i].ConnectedCrystals);
    }
}
