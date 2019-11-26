using CustomUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using PUN_Network;

namespace PUN_Network
{
    [RequireComponent(typeof(PUN_Lobby), typeof(PUN_Room), typeof(PUN_RoomSettings)),
     RequireComponent(typeof(PhotonView))]
    public class PUN_NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        PUN_Lobby _localLobby;
        PUN_Room _localRoom;
        Player _localPlayer;
        PUN_RoomSettings _defaultRoomSettings;
        PhotonView _photonView;
        UI_Manager _uiManager;
        GameObject _networkPlayer;

        public bool _isGameLoaded;
        public int _currentScene;
        public int _levelScene;
        public int _menuScene;

        public bool startGame;

        public PUN_Room GetRoom { get { return _localRoom; } }
        public PUN_Lobby GetLobby { get { return _localLobby; } }
        public GameObject GetNetworkPlayer { get { return _networkPlayer; } }
        public Player GetLocalPlayer { get { return _localPlayer; } }

        #endregion

        #region Methods

        private void Awake()
        {
            PhotonNetwork.OfflineMode = true;
            _uiManager = GameManager.MasterManager.UIManager;
            _localLobby = GetComponent<PUN_Lobby>();
            _localRoom = GetComponent<PUN_Room>();
            _defaultRoomSettings = GetComponent<PUN_RoomSettings>();
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.ConnectUsingSettings();
        }



        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log($"Player connected to Photon-Master-Server");
            PhotonNetwork.AutomaticallySyncScene = true;
            _localPlayer = PhotonNetwork.LocalPlayer;
            //JoinLobby();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            _currentScene = scene.buildIndex;
            if (_currentScene == _levelScene)
            {
                _isGameLoaded = true;
                _photonView.RPC("RPC_CreatePlayer", RpcTarget.AllViaServer);
            }
        }

        #region LobbyMethods

        public void JoinDefaultLobby()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log($"RoomUpdate called / Roomlist null: {roomList == null}");
            //base.OnRoomListUpdate(roomList);
            if (roomList != null && roomList.Count > 0)
            {
                Debug.Log($"Rooms not null");
                foreach (RoomInfo roomInfo in roomList)
                {
                    Debug.Log($"{roomInfo.ToString()}");
                    PUN_ServerlistEntry newLine = Instantiate(_uiManager?._serverEntryPrefab, _uiManager?._ServerList);
                    newLine.UpdateServerlistEntry(roomInfo);
                }
            }

        }

        #endregion

        #region RoomMethods

        /// <summary>
        /// Creates a random room. No setup needed.
        /// </summary>
        public void CreateRoom()
        {
            int randomRoomNumber = Random.Range(0, 1000);
            RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)_defaultRoomSettings.MaxPlayers, PublishUserId = true };
            PhotonNetwork.CreateRoom($"Room: {randomRoomNumber}", roomOptions, TypedLobby.Default);
        }

        /// <summary>
        /// Creates a custom Room with just the name given.
        /// </summary>
        /// <param name="roomName">The name that the host chose.</param>
        public void CreateRoom(string roomName)
        {
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)_defaultRoomSettings.MaxPlayers, PublishUserId = true };
            PhotonNetwork.CreateRoom(roomName, roomOps, TypedLobby.Default);
        }

        public ExitGames.Client.Photon.Hashtable SetRoomSettings()
        {
            ExitGames.Client.Photon.Hashtable customSettings = new ExitGames.Client.Photon.Hashtable()
            {
                //{"IsVisible", true },
                //{"IsOpen", true },
                //{"MaxPlayers", GameManager.MasterManager.UIManager._InputMaxPlayers }
            };

            return customSettings;
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            Debug.Log($"Created Room");
            //_localRoom.Room = PhotonNetwork.CurrentRoom;
            //_uiManager._RoomName.text = _localRoom.Room.Name;
            //Debug.Log($"Changed Roomname");
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            _localRoom.Room = PhotonNetwork.CurrentRoom;
            _uiManager._RoomName.text = _localRoom.Room.Name;
            Debug.Log($"Joined Room");
            if (_localRoom != null)
            {
                _localRoom.Players = _localRoom.UpdatePlayers();
                _localRoom.PlayersInRoom = _localRoom.Players.Length;
                _localRoom.MyNumberInRoom = _localRoom.PlayersInRoom;
                PhotonNetwork.NickName = _localRoom.MyNumberInRoom.ToString();
            }

            if (startGame == true)
            {
                photonView.RPC("RPC_StartGame", RpcTarget.AllViaServer);
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log($"Roomcreation failed, name must already exist");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.Log($"Failed to join, room did not exist");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log($"Called PlayerEnteredRoom");
            photonView.RPC("RPC_AddPlayerEntry", RpcTarget.AllBufferedViaServer);
            Debug.Log($"A new player entered: {newPlayer.NickName}");
            if (_localRoom.PlayersInRoom == _localRoom.GetRoomActiveSettings.MaxPlayers)
            {
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            else
            {
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }


        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            _localRoom.Room = null;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log($"{otherPlayer.NickName} has left the room");
        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
        }

        public void UpdateRoomSettings()
        {
            if (byte.TryParse(_uiManager._InputMaxPlayers.text, out byte maxPlayers))
            {
                _localRoom.Room.MaxPlayers = maxPlayers;
            }

        }

        #endregion

        #region RPCs

        [PunRPC]
        private void RPC_CreatePlayer()
        {
            this._networkPlayer = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NetworkPlayer"), transform.position, Quaternion.identity, 0);
        }

        [PunRPC]
        private void RPC_StartGame()
        {
            _isGameLoaded = true;
            if (!PhotonNetwork.IsMasterClient)
                return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(_levelScene);
            _uiManager.ToggleRoomMenu();
            _uiManager.ToggleMultiplayerMenu();
            _uiManager.ToggleHUD();
        }

        [PunRPC]
        public void SpawnUnit()
        {

        }

        [PunRPC]
        public void RPC_AddPlayerEntry()
        {
            PUN_PlayerlistEntry newLine = Instantiate(_uiManager?._playerEntryPrefab, _uiManager?._PlayerList);
            newLine.UpdatePlayerlistEntry(_localPlayer);
        }

        #endregion


        #endregion
    }
}
