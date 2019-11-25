using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(SphereCollider))]
public class Crystal : MonoBehaviourPun
{
    [SerializeField]
    TextAsset data;
    [SerializeField]
    private CrystalData _data;
    private int _id;
    private int _health;
    [SerializeField]
    private UnitData _unitData = null;
    [SerializeField]
    private GameObject _unitPrefab;
    //TODO: Save Units instead of Entities
    private readonly List<Unit> _unitsSpawned = new List<Unit>();

    public byte TeamID => _data.TeamID;
    public int Health
    {
        get => _health; private set
        {
            _health = value;
            if(_health <= 0)
            {
                _data.TeamID = 0;
                _health = 0;
            }
        }
    }
    public int ID { get => _id; private set => _id = value; }

    public Unit[] Units => _unitsSpawned.ToArray();

    public void Init()
    {
        Init(JsonUtility.FromJson<CrystalData>(data.text));
    }

    public void Init(GameObject prefab)
    {
        _unitPrefab = prefab;

        Init(JsonUtility.FromJson<CrystalData>(data.text));
    }

    //private void Start()
    //{
    //    //Init(JsonUtility.FromJson<CrystalData>(File.ReadAllText(string.Concat(Constants.CRYSTALDATA_PATH, "/Data.json"))));
    //}

    //private void Update()
    //{
    //    if(_unitsSpawned.Count >= _data.MaxUnitSpawned || TeamID == 0)
    //        _data.IsSpawning = false;
    //}

    private void Init(CrystalData data)
    {
        _data = data;
        Health = _data.MaxHealth;
        GetComponent<SphereCollider>().radius = _data.Range;
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

    [PunRPC]
    private void Spawn(float3 pos)
    {
        var unit = Instantiate(_unitPrefab, pos, Quaternion.identity).GetComponent<Unit>();

        _unitsSpawned.Add(unit);
    }

    public void Conquer(byte value, byte team)
    {
        if(TeamID != 0)
            Health -= value;
        else
        {
            Health += value;
            if(Health >= _data.MaxHealth)
            {
                Health = _data.MaxHealth;
                _data.TeamID = team;
            }
        }
    }
}
