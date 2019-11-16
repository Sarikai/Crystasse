using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField]
    TextAsset data;
    [SerializeField]
    private CrystalData _data;
    private int _id;
    private int _health;
    [SerializeField]
    private UnitData _unitData = null;
    private EntityArchetype _unitArchetype;
    private readonly List<Entity> _unitsSpawned = new List<Entity>();
    private readonly Dictionary<ID, Entity> _enemies = new Dictionary<ID, Entity>();

    public byte TeamID => _data.TeamID;
    public int Health { get => _health; private set => _health = value; }
    public int ID { get => _id; private set => _id = value; }

    private void Start()
    {
        Init(JsonUtility.FromJson<CrystalData>(data.text), UnitData.Archetype);
        //Init(JsonUtility.FromJson<CrystalData>(File.ReadAllText(string.Concat(Constants.CRYSTALDATA_PATH, "/Data.json"))), UnitData.Archetype);
    }

    private void Init(CrystalData data, EntityArchetype archetype)
    {
        _data = data;
        Health = _data.MaxHealth;
        _unitArchetype = archetype;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while(TeamID != 0 && _data.IsSpawning && _unitsSpawned.Count < _data.MaxUnitSpawned)
        {
            Spawn(new float3(UnityEngine.Random.Range(-4f, 4.1f), 0, UnityEngine.Random.Range(-4f, 4.1f)) + (float3)transform.position);
            yield return new WaitForSecondsRealtime(_data.SpawnRate);
        }
    }

    private void Spawn(float3 pos)
    {
        var e = EntitySpawnHelper.SpawnEntityWithValues(_unitArchetype, World.Active, _unitData);
        World.Active.EntityManager.SetComponentData(e, new Translation() { Value = pos });
        World.Active.EntityManager.SetSharedComponentData(e, new TeamID() { Value = TeamID });

        _unitsSpawned.Add(e);
    }
}
