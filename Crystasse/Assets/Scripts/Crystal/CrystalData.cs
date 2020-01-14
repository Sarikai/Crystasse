using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CrystalData
{
    public int MaxHealth;
    public int MaxUnitSpawned;

    public bool IsBase;
    public bool IsSpawning;
    public float SpawnRate;
    //public byte TeamID;
    public float Range;

    public CrystalData(int maxHealth, int maxUnitSpawned, float spawnRate, bool isBase, bool isSpawning, /*byte teamID,*/ float range)
    {
        MaxHealth = maxHealth;
        MaxUnitSpawned = maxUnitSpawned;

        SpawnRate = spawnRate;
        IsBase = isBase;
        IsSpawning = isSpawning;
        //TeamID = teamID;
        Range = range;
    }
}
