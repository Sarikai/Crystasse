using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal struct CrystalData
{
    public readonly int MaxHealth;
    public readonly int MaxUnitSpawned;

    public bool IsBase;
    public bool IsSpawning;
    public float SpawnRate;
    public byte TeamID;

    public CrystalData(int maxHealth, int maxUnitSpawned, float spawnRate, bool isBase, bool isSpawning, byte teamID)
    {
        MaxHealth = maxHealth;
        MaxUnitSpawned = maxUnitSpawned;

        SpawnRate = spawnRate;
        IsBase = isBase;
        IsSpawning = isSpawning;
        TeamID = teamID;
    }
}
