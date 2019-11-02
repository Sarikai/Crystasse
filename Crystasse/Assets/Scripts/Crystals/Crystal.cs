using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private CrystalData _data;
    private int _id;
    private int _health;
    private EntityArchetype _unitArchetype;
    private readonly List<Entity> _unitsSpawned = new List<Entity>();
    private readonly Dictionary<ID, Entity> _enemies = new Dictionary<ID, Entity>();

    //private EntityCommandBuffer _buffer;

    [SerializeField]
    private float _time = 1f, _timer = 0f;

    public byte TeamID => _data.TeamID;
    public int Health { get => _health; private set => _health = value; }
    public int ID { get => _id; private set => _id = value; }

    private void Start()
    {
        Init(JsonUtility.FromJson<CrystalData>(File.ReadAllText(string.Concat(Constants.CRYSTALDATA_PATH, "/Data.json"))), UnitData.Archetype);
    }

    private void Init(CrystalData data, EntityArchetype archetype)
    {
        _data = data;
        Health = _data.MaxHealth;
        _unitArchetype = archetype;
        //_buffer = World.Active.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_time <= _timer && _data.IsSpawning && TeamID != 0)
        {
            Spawn(transform.position);
            _timer = 0f;
        }
    }

    private void Spawn(float3 pos)
    {
        //var e = _buffer.CreateEntity(_unitArchetype);
        //_buffer.SetComponent(e, new Translation() { Value = transform.position });

        var e = TestScript.Instance.CreateEntity(World.Active);
        World.Active.EntityManager.SetComponentData<Translation>(e, new Translation() { Value = pos });

        _unitsSpawned.Add(e);
    }

    //private void OnDestroy()
    //{
    //    _buffer.Dispose();
    //}
}
