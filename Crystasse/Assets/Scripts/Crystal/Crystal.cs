using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using System.Linq;
using PUN_Network;

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
    [SerializeField]
    private List<Unit> _unitsSpawned = new List<Unit>();
    [SerializeField]
    private PhotonView _crystalView;
    [SerializeField]
    private int _viewID = 100;
    [SerializeField]
    private byte _teamID;

    Player _ownerPlayer;
    [SerializeField]
    MeshRenderer _crystalMeshRenderer;
    [SerializeField]
    Renderer _crystalRenderer;
    public MeshRenderer CrystalMeshRenderer { get => _crystalMeshRenderer; set => _crystalMeshRenderer = value; }

    private int isUpgraded = 0;
    private List<Unit> _unitsInRange = new List<Unit>();

    public byte TeamID => _teamID;
    public int Health
    {
        get => _health; private set
        {
            _health = value;
            if (_health <= 0)
            {
                _teamID = 0;
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
        _crystalView = GetComponent<PhotonView>();
        _crystalView.ViewID = _viewID;
    }




    public void SetCrystalView(Player player)
    {
        CrystalView.TransferOwnership(player);
    }

    public void Init(GameObject prefab)
    {
        _unitPrefab = prefab;
        //Init();
        photonView.RPC("Init", RpcTarget.AllViaServer);
    }


    [PunRPC]
    public void RPC_InitBaseCrystal()
    {
        if (_crystalView.IsMine)
        {
            //photonView.RPC("TransferTeamID", RpcTarget.AllViaServer);
            _teamID = GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID;
            GetComponentInChildren<MeshRenderer>().material = GameManager.MasterManager.CrystalMaterials[_teamID];
        }
        Health = _data.MaxHealth;
        _unitPrefab = _prefabDatabase[TeamID, isUpgraded];
        OnConquered += () => _unitPrefab = _prefabDatabase[TeamID, isUpgraded];
        OnConquered += _unitsSpawned.Clear;
        OnConquered += SetSpawningTrue;
        //OnConquered += ChangeTeam;
        OnConquered += () => StartCoroutine(ReworkedSpawnRoutine());
        GetComponent<SphereCollider>().radius = _data.Range;

        Debug.Log($"crystalview mine? {_crystalView.IsMine} &&  team mine? {IsMyTeam}");
        if (_crystalView.IsMine && IsMyTeam)
            StartCoroutine(ReworkedSpawnRoutine());
    }
    public void SetSpawningTrue()
    {
        _data.IsSpawning = true;
    }

    [PunRPC]
    public void RPC_InitSceneCrystal()
    {
        Debug.Log($"Init of this Crystal {CrystalView.ViewID} called");
        _teamID = 0;
        GetComponentInChildren<MeshRenderer>().material = GameManager.MasterManager.CrystalMaterials[0];
        Health = _data.MaxHealth;
        _unitPrefab = _prefabDatabase[TeamID, isUpgraded];
        OnConquered += () => _unitPrefab = _prefabDatabase[TeamID, isUpgraded];
        OnConquered += _unitsSpawned.Clear;
        OnConquered += SetSpawningTrue;
        //OnConquered += ChangeTeam; //Exfunc of GetComponentInChildren<MeshRenderer>().material = GameManager.MasterManager.CrystalMaterials[_teamID];
        OnConquered += () => StartCoroutine(ReworkedSpawnRoutine());
        GetComponent<SphereCollider>().radius = _data.Range;

        if (_crystalView.ViewID == 0 && _crystalView.IsSceneView)
            StartCoroutine(ReworkedSpawnRoutine());
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
            var pos = new Vector3(UnityEngine.Random.Range(-4f, 4.1f), 0, UnityEngine.Random.Range(-4f, 4.1f)) + transform.position;
            Debug.Log("Player: " + GameManager.MasterManager.NetworkManager.GetLocalPlayer.ActorNumber);
            var unit = PhotonNetwork.Instantiate(Constants.BASIC_UNIT_PREFAB_PATHS[TeamID], pos, Quaternion.identity).GetComponent<Unit>();
            _unitsSpawned.Add(unit);
            yield return new WaitForSecondsRealtime(_data.SpawnRate);
        }
    }



    [PunRPC]
    public void TransferTeamID()
    {
        _teamID = GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID;
    }



    public void Conquer(byte value, byte team)
    {
        //Debug.Log($"Entered conquer AP:{value}, AttackingTeam: {team}, ownTeam {TeamID}");
        if (TeamID != 0)
        {
            //Debug.Log($"Conquer");
            _data.IsSpawning = false;
            Health -= value;
        }
        else
        {
            //Debug.Log($"ConquerElse");
            Health += value;
            //GetComponentInChildren<MeshRenderer>().material = GameManager.MasterManager.CrystalMaterials[team];
            if (Health >= _data.MaxHealth)
            {
                //TODO: Change HUD data of Crystals owned here
                //TODO: [DONE] Assign attacking Team ID instead of localPlayer
                OwnerPlayer = GetPlayerOfTeam(team);
                Health = _data.MaxHealth;
                _teamID = team;
                GetComponentInChildren<MeshRenderer>().material = GameManager.MasterManager.CrystalMaterials[team];
                //_crystalMeshRenderer.material = GameManager.MasterManager.CrystalMaterials[team - 1];
                //Debug.Log($"Conquered null? {OnConquered == null}");

                if (IsMyTeam)
                {
                    CrystalConqueredSelf();
                }
                else
                {
                    CrystalConqueredEnemy();
                }

                if (OnConquered != null)
                    OnConquered.Invoke();
            }
        }

        if (Health <= 0)
        {
            _teamID = 0;
            CrystalNeutralized(team);
            GetComponentInChildren<MeshRenderer>().material = GameManager.MasterManager.CrystalMaterials[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Crystal Trigger Enter called by {other}");
        var unit = other.GetComponentInParent<Unit>();

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
        //Debug.Log($"Collider: {other}");
        var unit = other.GetComponentInParent<Unit>();
        //Debug.Log($"Crystal Trigger Stay called by {other} Checkresult{unit != null} && {unit.TeamID != TeamID} && {unit.CurrentState?.Type == States.Idle}");
        if (unit != null && unit.TeamID != TeamID && unit.CurrentState.Type == States.Idle)
            StateMachine.SwitchState(unit, new ConquerState(unit, this));
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"Crystal Trigger Exit called by {other}");
        if (other.GetComponentInParent<Unit>()?.TeamID == TeamID)
            _unitsInRange.Remove(other.GetComponentInParent<Unit>());
    }

    private void OnValidate()
    {
        if (data != null && string.IsNullOrWhiteSpace(data.text) && data.text.Equals(string.Empty))
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


    void CrystalNeutralized(byte team)
    {
        if (team == GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID)
        {
            GameManager.MasterManager.UIManager._crystalNeutral++;
            GameManager.MasterManager.UIManager._crystalEnemy--;
        }
        else
        {
            GameManager.MasterManager.UIManager._crystalNeutral++;
            GameManager.MasterManager.UIManager._crystalOwned--;
        }
        GameManager.MasterManager.UIManager.HUD_Update();
    }

    void CrystalConqueredSelf()
    {

        GameManager.MasterManager.UIManager._crystalNeutral--;
        GameManager.MasterManager.UIManager._crystalOwned++;
        GameManager.MasterManager.UIManager.HUD_Update();

    }

    void CrystalConqueredEnemy()
    {
        GameManager.MasterManager.UIManager._crystalNeutral--;
        GameManager.MasterManager.UIManager._crystalEnemy++;
        GameManager.MasterManager.UIManager.HUD_Update();
    }



    ///////////////////////////////
    //TODO: NEW THINGS TESTREGION//
    ///////////////////////////////

    #region New Funcs

    private IEnumerator ReworkedSpawnRoutine()
    {
        Debug.Log($"Started SpawnRoutine");
        Debug.Log($"{_data.IsSpawning} && {_unitsSpawned.Count < _data.MaxUnitSpawned} && {TeamID != 0} && {IsMyTeam}");
        while (_data.IsSpawning && _unitsSpawned.Count < _data.MaxUnitSpawned && TeamID != 0 && IsMyTeam /*&& _unitPrefab != null*/)
        {
            //Debug.Log($"Called Spanwloop");
            var pos = new Vector3(UnityEngine.Random.Range(-4f, 4.1f), 0, UnityEngine.Random.Range(-4f, 4.1f)) + transform.position;
            Unit unit;

            if (Constants.BASIC_UNIT_PREFAB_PATHS.Length > TeamID && Constants.BASIC_UNIT_PREFAB_PATHS[TeamID] != null)
            {
                var u = PhotonNetwork.Instantiate(Constants.BASIC_UNIT_PREFAB_PATHS[TeamID], pos, Quaternion.identity).GetComponent<Unit>();
                unit = u;
                GameManager.MasterManager.NetworkManager.SessionStats.IncrementSpawns();
            }
            else
            {
                //Debug.Log("Unit does not exist!");
                var u = PhotonNetwork.Instantiate(Constants.BASIC_UNIT_PREFAB_PATHS[1], pos, Quaternion.identity).GetComponent<Unit>();
                unit = u;
                GameManager.MasterManager.NetworkManager.SessionStats.IncrementSpawns();
                GameManager.MasterManager.NetworkManager.CustomPlayer.MatchSession.IncrementSpawns();
            }



            //TODO: think about spawned units stored, what happens on conquer with this spawned list (it's erased but units still exist) does it contain enemy units aswell (no does not)?

            _unitsSpawned.Add(unit);
            yield return new WaitForSecondsRealtime(_data.SpawnRate);
        }
        yield break;
    }

    public bool IsMyTeam
    {
        get
        {
            // Similar to PhotonView.IsMine
            return (this.TeamID == GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID) /*|| (PhotonNetwork.IsMasterClient && !this.IsOwnerActive)*/;
        }
    }

    public bool IsNeutral
    {
        get
        {
            // Similar to PhotonView.IsMine
            return (this.TeamID == 0) /*|| (PhotonNetwork.IsMasterClient && !this.IsOwnerActive)*/;
        }
    }

    public Renderer CrystalRenderer { get => _crystalRenderer; set => _crystalRenderer = value; }

    public Player GetPlayerOfTeam(int teamID)
    {
        PUN_CustomPlayer[] actualNetworkPlayers = FindObjectsOfType<PUN_CustomPlayer>();
        foreach (PUN_CustomPlayer networkPlayer in actualNetworkPlayers)
        {
            if (networkPlayer.TeamID == teamID)
                return networkPlayer.LocalPlayer;
        }
        return null;
    }


    #endregion
}
