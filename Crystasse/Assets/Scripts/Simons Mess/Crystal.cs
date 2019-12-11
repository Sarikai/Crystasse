using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

[RequireComponent(typeof(SphereCollider))]
public class Crystal : MonoBehaviourPunCallbacks, IPunObservable
{
    public event Action OnConquered;
    [SerializeField]
    InstantiateRandomMesh _randomMesh = null;
    [SerializeField]
    UnitPrefabDatabase _prefabDatabase = null;
    [SerializeField]
    TextAsset data;
    [SerializeField]
    private CrystalData _data;
    private int _id;
    [SerializeField]
    private int _health;
    //[SerializeField]
    //private UnitData _unitData = null;
    private GameObject _unitPrefab;
    private readonly List<Unit> _unitsSpawned = new List<Unit>();
    [SerializeField]
    private PhotonView _crystalView;

    private int isUpgraded = 0;
    private List<Unit> _unitsInRange = new List<Unit>();

    public byte TeamID => _data.TeamID;
    public int Health
    {
        get => _health; private set
        {
            _health = value;
            if (_health <= 0)
            {
                _data.TeamID = 0;
                _health = 0;
            }
        }
    }
    public int ID { get => _id; private set => _id = value; }

    public Unit[] Units => _unitsSpawned.ToArray();

    public CrystalData Data => _data;

    //private void Awake()
    //{
    //    SetCrystalView(GameManager.MasterManager.NetworkManager.GetLocalPlayer);
    //}



    public void SetCrystalView(Player player)
    {
        _crystalView.TransferOwnership(player);
    }

    public void Init(GameObject prefab)
    {
        _unitPrefab = prefab;

        Init();
    }



    public void Init()
    {
        if (_data.IsBase)
            //TODO: In GM als Base eintragen
            ;
        Health = _data.MaxHealth;
        _unitPrefab = _prefabDatabase[TeamID, isUpgraded];
        OnConquered += () => _randomMesh.InstantiateMesh();
        OnConquered += () => _unitPrefab = _prefabDatabase[TeamID, isUpgraded];
        //OnConquered += () => StopCoroutine(SpawnRoutine());
        OnConquered += () => _unitsSpawned.Clear();
        OnConquered += () => StartCoroutine(SpawnRoutine());
        GetComponent<SphereCollider>().radius = _data.Range;
        //TODO: PV Comparison (Is this my Crystal?)
        StartCoroutine(SpawnRoutine());
    }

    public void UpdateCrystal()
    {
        if (_unitsInRange.Count >= Constants.UNITCOUNT_FOR_UPGRADE)
        {
            isUpgraded = 1;
            for (int i = 0; i < _unitsInRange.Count; i++)
                _unitsInRange[i].TakeDamage(_unitsInRange[i].Health);
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (_data.IsSpawning && _unitsSpawned.Count < _data.MaxUnitSpawned && TeamID != 0 && _unitPrefab != null)
        {
            _crystalView.RPC("Spawn", RpcTarget.AllViaServer, new float3(UnityEngine.Random.Range(-4f, 4.1f), 0, UnityEngine.Random.Range(-4f, 4.1f)) + (float3)transform.position);
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
        Debug.Log("Conquer: " + value + " TID: " + team);
        if (TeamID != 0)
            Health -= value;
        else
        {
            Health += value;
            if (Health >= _data.MaxHealth)
            {
                Health = _data.MaxHealth;
                _data.TeamID = team;
                if (OnConquered != null)
                    OnConquered.Invoke();
            }
        }

        if (Health <= 0)
        {
            _data.TeamID = 0;
            _randomMesh.InstantiateMesh();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponent<Unit>();

        if (unit != null)
        {
            if (unit.TeamID == TeamID)
                _unitsInRange.Add(unit);
            else
                StateMachine.SwitchState(unit, new ConquerState(unit, this));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var unit = other.GetComponent<Unit>();
        if (unit != null && unit.TeamID != TeamID && unit.CurrentState.Type == States.Idle)
            StateMachine.SwitchState(unit, new ConquerState(unit, this));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Unit>()?.TeamID == TeamID)
            _unitsInRange.Remove(other.GetComponent<Unit>());
    }

    private void OnValidate()
    {
        if (data != null)
            _data = JsonUtility.FromJson<CrystalData>(data.text);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }
}
