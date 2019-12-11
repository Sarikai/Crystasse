using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[CreateAssetMenu()]
public class Map : ScriptableObject
{
    [Header("Team 1"), SerializeField]
    public GameObject[] _team1 = null;
    [Header("Team 2"), SerializeField]
    public GameObject[] _team2 = null;

    [Header("Bases"), SerializeField]
    public GameObject[] _bases = new GameObject[2];
}
