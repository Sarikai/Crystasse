using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private static EntityManager _entityManager;
    [SerializeField]
    TextAsset data;
    [SerializeField]
    private CrystalData _data;
    private int _id;
    private int _health;
    [SerializeField]
    private UnitData _unitData = null;
    [SerializeField]
    private CrystalEntityData _crystalEntityData = null;
    private GameObject _unitPrefab;
    private EntityArchetype _unitArchetype;
    //TODO: Save Units instead of Entities
    private readonly List<Unit> _unitsSpawned = new List<Unit>();

    public byte TeamID => _data.TeamID;
    public int Health { get => _health; private set => _health = value; }
    public int ID { get => _id; private set => _id = value; }

    public Unit[] Units => _unitsSpawned.ToArray();


    public void Init(GameObject prefab)
    {
        _unitPrefab = prefab;

        if(_entityManager == null)
            _entityManager = World.Active.EntityManager;

        Init(JsonUtility.FromJson<CrystalData>(data.text), UnitData.Archetype);
    }

    //private void Start()
    //{
    //    //Init(JsonUtility.FromJson<CrystalData>(File.ReadAllText(string.Concat(Constants.CRYSTALDATA_PATH, "/Data.json"))), UnitData.Archetype);
    //}

    private void Update()
    {
        if(_unitsSpawned.Count >= _data.MaxUnitSpawned || TeamID == 0)
            _data.IsSpawning = false;
    }

    private void Init(CrystalData data, EntityArchetype archetype)
    {
        _data = data;
        Health = _data.MaxHealth;
        _unitArchetype = archetype;
        EntitySpawnHelper.SpawnEntityWithValues(CrystalEntityData.Archetype, _entityManager, _crystalEntityData);
        //TODO: PV Comparison (Is this my Crystal?)
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while(_data.IsSpawning && _unitsSpawned.Count < _data.MaxUnitSpawned && TeamID != 0)
        {
            Spawn(new float3(UnityEngine.Random.Range(-4f, 4.1f), 0, UnityEngine.Random.Range(-4f, 4.1f)) + (float3)transform.position);
            yield return new WaitForSecondsRealtime(_data.SpawnRate);
        }
    }

    private void Spawn(float3 pos)
    {
        var e = EntitySpawnHelper.SpawnEntityWithValues(_unitArchetype, _entityManager, _unitData);
        _entityManager.SetComponentData(e, new Translation() { Value = pos });
        _entityManager.SetSharedComponentData(e, new TeamID() { Value = TeamID });

        var unit = Instantiate(_unitPrefab, pos, Quaternion.identity).GetComponent<Unit>();

        _unitsSpawned.Add(unit);
    }
}
