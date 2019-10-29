using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private CrystalData _data;
    private int _id;
    private int _health;
    private EntityArchetype _unitArchetype;
    private readonly List<Entity> _unitsSpawned = new List<Entity>();
    private readonly Dictionary<ID, Entity> _enemies = new Dictionary<ID, Entity>();

    public byte TeamID => _data.TeamID;
    public int Health { get => _health; private set => _health = value; }
    public int ID { get => _id; private set => _id = value; }

    private void Init(CrystalData data, EntityArchetype archetype)
    {
        _data = data;
        Health = _data.MaxHealth;
        _unitArchetype = archetype;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while(_data.IsSpawning && TeamID != 0)
        {
            _unitsSpawned.Add(World.Active.EntityManager.CreateEntity(_unitArchetype));
            yield return new WaitForSecondsRealtime(_data.SpawnRate);
        }
    }
}
