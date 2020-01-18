using CustomUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PUN_Network;

namespace PUN_Network
{
    //[RequireComponent(typeof(PhotonView), typeof(InputManager))]
    public class PUN_CustomPlayer : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties


        //Variables
        [SerializeField]
        private string _crystalPrefab;                 //string cause of memory location
        [SerializeField]
        private Player _localPlayer;                   //
        [SerializeField]
        private string _nickName = "New Enligthened";  //
        [SerializeField]
        private byte _teamID;                          //selected Team ID //TODO: implement correct selection
        [SerializeField]
        private int _actorNumber;                      //PhotonNetworks ID given in a room, outside of rooms -1
        [SerializeField]
        private string _unitPrefab;                    //string cause of memory location
        [SerializeField]
        private Match _matchSession;                   //counts statistic of one match, be sure to clear after win/loss //TODO: new match
        [SerializeField]
        private PhotonView _customPlayerView;          //view that controls while ingame
        [SerializeField]
        private PUN_PlayerlistEntry _playerlistEntry;


        //Properties
        public int ActorNumber { get => _actorNumber; set => _actorNumber = value; }
        public byte TeamID { get => _teamID; set => _teamID = value; }
        public string NickName { get => _nickName; set => _nickName = value; }
        public Player LocalPlayer { get => _localPlayer; set => _localPlayer = value; }
        public string CrystalPrefab { get => _crystalPrefab; set => _crystalPrefab = value; }
        public string UnitPrefab { get => _unitPrefab; set => _unitPrefab = value; }
        public PhotonView CustomPlayerView { get => _customPlayerView; set => _customPlayerView = value; }
        public PUN_PlayerlistEntry PlayerlistEntry { get => _playerlistEntry; set => _playerlistEntry = value; }

        #endregion

        #region Methods

        private void Awake()
        {
            Debug.Log($"CustomPlayerInstance awake");
            DontDestroyOnLoad(this);
            _customPlayerView = GetComponent<PhotonView>();
            //_customPlayerView.ViewID = Random.Range(1500, 1600);
            GameManager.MasterManager.InputManager = GetComponent<InputManager>();
            //_customPlayerView.RPC("PUN_InitCustomPlayer", RpcTarget.AllViaServer, GameManager.MasterManager.NetworkManager.GetLocalPlayer);
            //InitCustomPlayer();
            //TODO: Init InputManager
        }

        private void InitCustomPlayer()
        {
            Debug.Log($"CustomPlayer init called");
            _crystalPrefab = GameManager.MasterManager._crystalPrefabLocation;
            _unitPrefab = GameManager.MasterManager._unitPrefabLocation;
            _localPlayer = PhotonNetwork.LocalPlayer;
            Debug.Log($"Local Player Actor Number: {_localPlayer.ActorNumber}");
            _teamID = (byte)(_localPlayer.ActorNumber /*+ 1*/);
            GameManager.MasterManager.InputManager._teamID = _teamID;
            //_nickName = _nickName;

            //TODO: Outsorce from Init or its called double, maybe ownercheck?
            //_playerlistEntry = PhotonNetwork.Instantiate(Constants.NETWORKED_UI_ELEMENTS[0], Vector3.zero, Quaternion.identity)?.GetComponent<PUN_PlayerlistEntry>();
            //Debug.Log($"Player entry instatiated");
            //_playerlistEntry.transform.SetParent(GameManager.MasterManager.UIManager._PlayerList.transform);
            //_playerlistEntry.UpdatePlayerlistEntry(this);

        }

        [PunRPC]
        private void PUN_InitCustomPlayer(Player player)
        {
            Debug.Log($"PUN_CustomPlayer init called");
            _crystalPrefab = GameManager.MasterManager._crystalPrefabLocation;
            _unitPrefab = GameManager.MasterManager._unitPrefabLocation;
            _localPlayer = player;
            Debug.Log($"Local Player Actor Number: {player.ActorNumber}");
            _teamID = (byte)(player.ActorNumber /*+ 1*/);
            GameManager.MasterManager.InputManager._teamID = _teamID;
            //_nickName = _nickName;

            //TODO: Outsorce from Init or its called double, maybe ownercheck?
            //_playerlistEntry = PhotonNetwork.Instantiate(Constants.NETWORKED_UI_ELEMENTS[0], Vector3.zero, Quaternion.identity)?.GetComponent<PUN_PlayerlistEntry>();
            //Debug.Log($"Player entry instatiated");
            //_playerlistEntry.transform.SetParent(GameManager.MasterManager.UIManager._PlayerList.transform);
            //_playerlistEntry.UpdatePlayerlistEntry(this);

        }




        #region RPCs



        #endregion


        #endregion
    }
}
