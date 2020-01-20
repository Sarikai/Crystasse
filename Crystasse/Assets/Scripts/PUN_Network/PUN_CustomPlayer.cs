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
    public class PUN_CustomPlayer : MonoBehaviourPunCallbacks, IPunObservable
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
            //GameManager.MasterManager.InputManager = GetComponent<InputManager>();
            //_customPlayerView.RPC("PUN_InitCustomPlayer", RpcTarget.AllViaServer, GameManager.MasterManager.NetworkManager.GetLocalPlayer);
            //InitCustomPlayer();
            //TODO: [DONE] Init InputManager
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
            _actorNumber = player.ActorNumber;
            //TODO: check for remove
            //_actorNumber = player.ActorNumber;
            if (IsMyCustomPlayer)
            {
                GameManager.MasterManager.InputManager = GetComponent<InputManager>();
                Debug.Log($"My Team ID { _teamID}");
                GameManager.MasterManager.InputManager._teamID = _teamID;
                _playerlistEntry = PhotonNetwork.Instantiate(Constants.NETWORKED_UI_ELEMENTS[0], Vector3.zero, Quaternion.identity)?.GetComponent<PUN_PlayerlistEntry>();
                Debug.Log($"player entry instatiated");
                CustomPlayerView.RPC("RPC_InitPlayerlistEntry", RpcTarget.AllBufferedViaServer, CustomPlayerView.ViewID, _playerlistEntry.photonView.ViewID, player);
                //_playerlistEntry.transform.SetParent(GameManager.MasterManager.UIManager._PlayerList.transform);
                //_playerlistEntry.UpdatePlayerlistEntry(this);
            }
            //_nickName = _nickName;         
            //_playerlistEntry.UpdatePlayerlistEntry(this);

        }

        public bool IsMyCustomPlayer
        {
            get
            {
                // Similar to PhotonView.IsMine
                //Debug.Log($"IsMyCustomPlayer: {CustomPlayerView.CreatorActorNr == LocalPlayer.ActorNumber}");
                return (CustomPlayerView.CreatorActorNr == PhotonNetwork.LocalPlayer.ActorNumber) /*|| (PhotonNetwork.IsMasterClient && !this.IsOwnerActive)*/;
            }
        }


        #region RPCs

        [PunRPC]
        public void RPC_InitPlayerlistEntry(int CustomPlayerViewID, int EntryViewID, Player player)
        {
            PUN_PlayerlistEntry entryObject = PhotonView.Find(EntryViewID).gameObject.GetComponent<PUN_PlayerlistEntry>();
            PUN_CustomPlayer customPlayerObject = PhotonView.Find(CustomPlayerViewID).gameObject.GetComponent<PUN_CustomPlayer>();
            Debug.Log($"Name of GO {entryObject.name}");
            //newEntry.transform.SetParent(GameManager.MasterManager.UIManager._PlayerList.transform);
            /*PUN_PlayerlistEntry saveEntry = */
            entryObject.UpdatePlayerlistEntry(player);
            customPlayerObject.PlayerlistEntry = entryObject;
            //entryObject.transform.SetParent(GameManager.MasterManager.UIManager._PlayerList.transform);
            //saveEntry.UpdatePlayerlistEntry(player);
            //_uiManager._PlayerName.text = newPlayer.NickName;
            //GameManager.MasterManager.NetworkManager._playerListEntries.Add(player, entryObject.GetComponent<PUN_PlayerlistEntry>().gameObject);
            GameManager.MasterManager.NetworkManager._playerListEntries.Add(customPlayerObject, entryObject.GetComponent<PUN_PlayerlistEntry>().gameObject);

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(TeamID);
                Debug.Log($"LocalClient sending teamID {GetComponent<PhotonView>().ViewID}");
            }
            else
            {
                this.TeamID = (byte)stream.ReceiveNext();
                Debug.Log($"LocalClient receiving teamID {GetComponent<PhotonView>().ViewID}");
            }
        }

        #endregion

        #endregion
    }




}

