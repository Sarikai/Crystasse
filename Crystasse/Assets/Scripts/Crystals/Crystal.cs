using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private CrystalData _data;
    private int _id;
    private int _health;
    private Unit _unitPrefab;
    //TODO: Make ECS Compatible
    private List<Unit> _unitsSpawned;
    private Dictionary<UnitID, Unit> _enemies;

    public byte TeamID => _data.TeamID;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while(_data.IsSpawning && TeamID != 0)
        {
            Debug.Log("Spawn");
            yield return new WaitForSecondsRealtime(_data.SpawnRate);
        }
    }
}
