using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class UnitPrefabDatabase : ScriptableObject
{
    [Header("Team 1"), SerializeField]
    GameObject[] _team1Prefabs = new GameObject[2];
    [Header("Team 2"), SerializeField]
    GameObject[] _team2Prefabs = new GameObject[2];

    public GameObject this[byte team, int i]
    {
        get
        {
            if(i <= 1)
            {
                if(team == 1)
                    return _team1Prefabs[i];
                if(team == 2)
                    return _team2Prefabs[i];

                return null;
            }
            else
                throw new System.IndexOutOfRangeException();
        }
    }
}
