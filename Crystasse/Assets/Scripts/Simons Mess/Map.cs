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

    public Crystal[] LoadMap(out List<Crystal> bases)
    {
        bases = new List<Crystal>(2);
        Debug.Log(_bases.Length);
        bases.Add(GameObject.Instantiate(_bases[0]).GetComponent<Crystal>());
        bases.Add(GameObject.Instantiate(_bases[1]).GetComponent<Crystal>());

        var list = new List<Crystal>();

        foreach(var c in _team1)
        {
            list.Add(GameObject.Instantiate(c).GetComponent<Crystal>());
        }
        foreach(var c in _team2)
        {
            list.Add(GameObject.Instantiate(c).GetComponent<Crystal>());
        }

        return list.ToArray();
    }
}
