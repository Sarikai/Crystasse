using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using System.Linq;

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
    private List<Unit> _unitsSpawned = new List<Unit>();
    [SerializeField]
    private PhotonView _crystalView;
    [SerializeField]
    private int _viewID = 100;

    Player _ownerPlayer;

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

    public Player OwnerPlayer
    {
        get => _ownerPlayer;
        set
        {
            _ownerPlayer = value;
            CrystalView.TransferOwnership(_ownerPlayer);
        }
    }

    public PhotonView CrystalView { get => _crystalView; private set => _crystalView = value; }

    private void Awake()
    {
        CrystalView.ViewID = _viewID;
    }



    public void SetCrystalView(Player player)
    {
        CrystalView.TransferOwnership(player);
    }

    public void Init(GameObject prefab)
    {
        _unitPrefab = prefab;

        Init();
    }



    public void Init()
    {
        Health = _data.MaxHealth;
        _unitPrefab = _prefabDatabase[TeamID, isUpgraded];
        OnConquered += () => _unitPrefab = _prefabDatabase[TeamID, isUpgraded];
        OnConquered += _unitsSpawned.Clear;
        OnConquered += ChangeTeam;
        OnConquered += () => StartCoroutine(SpawnRoutine());
        GetComponent<SphereCollider>().radius = _data.Range;

        if (_crystalView.IsMine)
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

    //private IEnumerator SpawnRoutine()
    //{
    //    while(_data.IsSpawning && _unitsSpawned.Count < _data.MaxUnitSpawned && TeamID != 0 && _unitPrefab != null)
    //    {
    //        var pos = new Vector3(UnityEngine.Random.Range(-4f, 4.1f), 0, UnityEngine.Random.Range(-4f, 4.1f)) + transform.position;
    //        CrystalView.RPC("Spawn", RpcTarget.AllViaServer, pos);

    //        yield return new WaitForSecondsRealtime(_data.SpawnRate);
    //    }
    //}

    private IEnumerator SpawnRoutine()
    {
        while (_data.IsSpawning && _unitsSpawned.Count < _data.MaxUnitSpawned && TeamID != 0 && _unitPrefab != null)
        {
            var pos = new Vector3(UnityEngine.Random.Range(-4f, 4.1f), 0, UnityEngine.Random.Range(-4f, 4.1f)) + transform.position;
            //CrystalView.RPC("Spawn", RpcTarget.AllViaServer, pos);
            Debug.Log("Player: " + GameManager.MasterManager.NetworkManager.GetLocalPlayer.ActorNumber);
            var unit = PhotonNetwork.Instantiate(Constants.BASIC_UNIT_PREFAB_PATHS[TeamID], pos, Quaternion.identity).GetComponent<Unit>();
            _unitsSpawned.Add(unit);
            yield return new WaitForSecondsRealtime(_data.SpawnRate);
        }
    }

    [PunRPC]
    private void Spawn(Vector3 pos)
    {
        var unit = Instantiate(_unitPrefab, pos, Quaternion.identity).GetComponent<Unit>();
        _unitsSpawned.Add(unit);
        if (_unitsSpawned.Count > 0)
            CrystalView.RPC("RPC_SetUnitView", RpcTarget.AllViaServer, _unitsSpawned.Count - 1);
    }


    [PunRPC]
    public void RPC_SetUnitView(int id)
    {
        _unitsSpawned[id]._view.TransferOwnership(OwnerPlayer);
    }

    public void Conquer(byte value, byte team)
    {
        if (TeamID != 0)
            Health -= value;
        else
        {
            Health += value;
            if (Health >= _data.MaxHealth)
            {
                OwnerPlayer = PhotonNetwork.LocalPlayer;
                Health = _data.MaxHealth;
                _data.TeamID = team;
                if (OnConquered != null)
                    OnConquered.Invoke();
            }
        }

        if (Health <= 0)
        {
            _data.TeamID = 0;
            ChangeTeam();
        }
    }

    private void ChangeTeam()
    {
        _randomMesh.ChangeMaterial(_crystalView);
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
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
            stream.SendNext(_unitsSpawned.Count);
            //foreach (var unit in _unitsSpawned)
            //{
            //    stream.SendNext(JsonUtility.ToJson(unit));
            //}
        }
        else
        {
            this.Health = (int)stream.ReceiveNext();
            int count = (int)stream.ReceiveNext();

            //for (int i = 0; i < count; i++)
            //{
            //    _unitsSpawned.Add(JsonUtility.FromJson<Unit>((string)stream.ReceiveNext()));
            //}
            //this._unitsSpawned.Clear();
            //this._unitsSpawned.AddRange((Unit[])stream.ReceiveNext());

        }
    }


    void CrystalNeutralized()
    {

    }

    void CrystalConqueredSelf()
    {

        GameManager.MasterManager.UIManager._crystalNeutral--;
        GameManager.MasterManager.UIManager._crystalOwned++;

    }

    void CrystalConqueredEnemy()
    {
        GameManager.MasterManager.UIManager._crystalNeutral--;
        GameManager.MasterManager.UIManager._crystalEnemy++;
    }
}
