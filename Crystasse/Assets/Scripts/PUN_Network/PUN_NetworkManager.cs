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
using System.Linq;

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
        private GameObject _networkPlayer;

        [SerializeField]
        PUN_CustomPlayer _customPlayer;
        string _customPlayerPref = "NetworkPlayer";

        public bool _isGameLoaded;
        public int _currentScene;
        public int _levelScene;
        public int _menuScene;

        public bool startGame;

        public PUN_Room GetRoom { get { return _localRoom; } }
        public PUN_Lobby GetLobby { get { return _localLobby; } }
        public GameObject GetNetworkPlayer { get { return _networkPlayer; } }
        public Player GetLocalPlayer { get { return _localPlayer; } }

        public PUN_CustomPlayer CustomPlayer { get => _customPlayer; set => _customPlayer = value; }


        //
        public Dictionary<int, GameObject> _serverListEntries = new Dictionary<int, GameObject>();
        public Dictionary<PUN_CustomPlayer, GameObject> _playerListEntries = new Dictionary<PUN_CustomPlayer, GameObject>();
        public Dictionary<Match, GameObject> _matchEntries = new Dictionary<Match, GameObject>();
        public Stats MatchStats;

        //Additional things?
        public List<Unit> units = new List<Unit>();
        public List<Crystal> crystals = new List<Crystal>();
        public List<Crystal> baseCrystals = new List<Crystal>();
        public List<Crystal> unassignedBaseCrystals = new List<Crystal>();
        public List<Player> unassignedPlayers = new List<Player>();

        #endregion

        #region Methods

        private void Awake()
        {
            Debug.Log($"NetworkManager Awake started");
            //PhotonNetwork.OfflineMode = true;
            _uiManager = GameManager.MasterManager.UIManager;
            _localLobby = GetComponent<PUN_Lobby>();
            _localRoom = GetComponent<PUN_Room>();
            _defaultRoomSettings = GetComponent<PUN_RoomSettings>();
            _photonView = GetComponent<PhotonView>();
            Debug.Log($"NetworkManager Awake end");
        }

        private void Start()
        {
            //PhotonNetwork.OfflineMode = false;
            PhotonNetwork.ConnectUsingSettings();
        }

        //public void Update()
        //{
        //    if (startGame == true)
        //    {
        //        photonView.RPC("RPC_StartGame", RpcTarget.AllViaServer);
        //    }
        //}

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log($"Player connected to Photon-Master-Server");
            PhotonNetwork.AutomaticallySyncScene = true;

            _localPlayer = PhotonNetwork.LocalPlayer;
            Debug.Log($"Local player ID: {_localPlayer.UserId}");
            JoinDefaultLobby();
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
                //PhotonNetwork.Instantiate(_customPlayerPref, Vector3.zero, Quaternion.identity);
                //GameManager.MasterManager.LoadMap();
                _uiManager.Toggle(_uiManager._Background);
                GameManager.MasterManager.SoundManager.IngameMusic();
                GameManager.MasterManager.DoUpdate = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    //TODO: [DONE] Assigning crystal views to crystal and change owner
                    //photonView.RPC("RPC_SetCrystalViews", RpcTarget.AllViaServer, PhotonNetwork.PlayerList);
                    crystals.AddRange(FindObjectsOfType<Crystal>());
                    for (int i = 0; i < crystals.Count; i++)
                    {
                        //crystals[i].CrystalView.ViewID = 100 + i;
                        if (crystals[i].Data.IsBase)
                            baseCrystals.Add(crystals[i]);
                    }

                    AssignStartCrystals();

                }
            }
        }

        #region LobbyMethods

        public void JoinDefaultLobby()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log($"RoomUpdate called / Roomlist null: {roomList == null}" + " Count: " + roomList.Count);
            base.OnRoomListUpdate(roomList);
            //ClearServerEntries();
            if (roomList != null && roomList.Count > 0)
            {
                Debug.Log($"Rooms not null");
                foreach (RoomInfo roomInfo in roomList)
                {
                    if (!_serverListEntries.ContainsKey(roomInfo.ID))
                    {
                        Debug.Log($"{roomInfo.ToString()}");
                        PUN_ServerlistEntry newLine = Instantiate(_uiManager?._serverEntryPrefab, _uiManager?._ServerList);
                        newLine.UpdateServerlistEntry(roomInfo);
                        _serverListEntries.Add(roomInfo.ID, newLine.gameObject);
                    }
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
        }

        public bool JoinRoom()
        {
            return PhotonNetwork.JoinRandomRoom();
        }

        public bool JoinRoom(string roomName)
        {
            return PhotonNetwork.JoinRoom(roomName);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log($"Joined Room");

            //TODO: [DONE] Prüfen ob auf beiden Seiten instanziert wird!
            //1 - Instanzieren des NetworkPlayers
            //2 - Init (Zuweisung: _crystalPrefab, _localPlayer->PUN Player, _nickName, _teamID, _actorNumber, _unitPrefab,_matchSession,_customPlayerView;
            _customPlayer = PhotonNetwork.Instantiate(_customPlayerPref, Vector3.zero, Quaternion.identity)?.GetComponent<PUN_CustomPlayer>();
            //Debug.Log($"Custom player added {CustomPlayer}/n Owner: {CustomPlayer.CustomPlayerView.Owner}");
            _customPlayer.CustomPlayerView.RPC("RPC_InitCustomPlayer", RpcTarget.AllBufferedViaServer, GetLocalPlayer);
            //_customPlayer.PlayerlistEntry = PhotonNetwork.Instantiate(Constants.NETWORKED_UI_ELEMENTS[0], Vector3.zero, Quaternion.identity)?.GetComponent<PUN_PlayerlistEntry>();
            //_customPlayer.Init(); --> is called in its awake
            //_customPlayer.CrystalPrefab = GameManager.MasterManager._crystalPrefabLocation; 
            //_customPlayer.UnitPrefab = GameManager.MasterManager._unitPrefabLocation;





            _localRoom.Room = PhotonNetwork.CurrentRoom;
            _uiManager._RoomName.text = _localRoom.Room.Name;
            if (_localRoom != null)
            {

                _localRoom.Players = _localRoom.UpdatePlayers();
                _localRoom.PlayersInRoom = _localRoom.Room.PlayerCount;
                _localRoom.MyNumberInRoom = _localRoom.Room.PlayerCount;
                PhotonNetwork.NickName = "Player" + _localRoom.MyNumberInRoom.ToString();
                //photonView.RPC("RPC_AddPlayerEntry", RpcTarget.AllBufferedViaServer, _localPlayer);
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
            //Debug.Log($"Called PlayerEnteredRoom");
            //photonView.RPC("RPC_AddPlayerEntry", RpcTarget.AllBufferedViaServer, newPlayer);
            //_localRoom.Players = _localRoom.UpdatePlayers();
            Debug.Log($"A new player entered: {newPlayer.NickName}");
            _localRoom.Players = _localRoom.UpdatePlayers();
            if (_localRoom.PlayersInRoom == _localRoom.GetRoomActiveSettings.MaxPlayers && PhotonNetwork.IsMasterClient)
            {
                //if (!PhotonNetwork.IsMasterClient)
                //    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            else
            {
                //if (!PhotonNetwork.IsMasterClient)
                //    return;
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }
        }

        public void LeaveRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
                for (int i = 0; i < players.Count; i++)
                {
                    if (players.ContainsKey(i) && PhotonNetwork.CurrentRoom.MasterClientId != players[i].ActorNumber)
                    {
                        _photonView.RPC("RPC_RemovePlayerEntry", RpcTarget.Others, PhotonNetwork.LocalPlayer);
                        _photonView.RPC("RPC_RemovePlayerEntry", RpcTarget.All, players[i]);
                        PhotonNetwork.CloseConnection(players[i]);
                    }
                }
            }
            else
            {
                _photonView.RPC("RPC_RemovePlayerEntry", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
                PhotonNetwork.LeaveRoom();
                _localRoom.Room = null;
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            RemovePlayerEntry(otherPlayer);
            Debug.Log($"{otherPlayer.NickName} has left the room");
        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
        }

        public void OnStartGame()
        {
            _photonView.RPC("RPC_StartGame", RpcTarget.AllViaServer);
        }

        public void UpdateRoomSettings()
        {
            if (byte.TryParse(_uiManager._InputMaxPlayers.text, out byte maxPlayers))
            {
                _localRoom.Room.MaxPlayers = maxPlayers;
            }

        }

        private void ClearServerEntries()
        {
            for (int i = 0; i < _serverListEntries.Count; i++)
            {
                GameObject entry = _serverListEntries[i];
                _serverListEntries.Remove(i);
                Destroy(entry);
            }
        }

        //private void ClearPlayerEntries()
        //{
        //    foreach (Player entry in _serverListEntries)
        //    {
        //        _playerListEntries.Remove(entry);
        //        Destroy(entry.ga);
        //    }
        //}

        private void RemovePlayerEntry(Player leavingPlayer)
        {
            //TODO: Test if change worked!
            GameObject entryToRemove = _playerListEntries[GetNetworkPlayerReference(leavingPlayer)];
            _playerListEntries.Remove(GetNetworkPlayerReference(leavingPlayer));
            Destroy(entryToRemove);
        }

        public void SetCrystalViews(Player[] players)
        {
            Crystal randomCrystal;
            foreach (Player player in players)
            {
                randomCrystal = GameManager.MasterManager.bases[Random.Range(0, GameManager.MasterManager.bases.Count)];
                randomCrystal.SetCrystalView(player);
                GameManager.MasterManager.bases.Remove(randomCrystal);
            }
            GameManager.MasterManager.StartInitCrystals();
        }

        public void AssignStartCrystals()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.PlayerList.Length > 1)
                    unassignedPlayers.AddRange(PhotonNetwork.PlayerList);
                else
                    unassignedPlayers.Add(PhotonNetwork.PlayerList[0]);

                foreach (Player unassignedPlayer in unassignedPlayers)
                {
                    //Player player = unassignedPlayer;
                    AssignRandomBaseCrystal(unassignedPlayer);
                    //unassignedPlayers.Remove(unassignedPlayer);


                    //if (crystals[i].Data.IsBase && crystals[i].CrystalView.OwnerActorNr == 0 || crystals[i].Data.IsBase && crystals[i].CrystalView.OwnerActorNr == p - 1)
                    //       {
                    //           crystals[i].CrystalView.TransferOwnership(PhotonNetwork.PlayerList[p]);
                    //           crystals[i].Init();
                    //      }
                    //TODO: [DONE] Crystal TeamID PUN call change?
                }
            }
        }

        public void AssignRandomBaseCrystal(Player targetPlayer)
        {
            unassignedBaseCrystals = baseCrystals;
            if (unassignedBaseCrystals != null && unassignedBaseCrystals[0] != null && unassignedBaseCrystals.Count >= 1)
            {
                int rndCrystal = Random.Range(0, unassignedBaseCrystals.Count);
                unassignedBaseCrystals[rndCrystal].CrystalView.TransferOwnership(targetPlayer);
                //unassignedBaseCrystals[rndCrystal].Init();
                unassignedBaseCrystals[rndCrystal].photonView.RPC("Init", RpcTarget.AllViaServer);
                unassignedBaseCrystals.RemoveAt(rndCrystal);
            }
        }

        public PUN_CustomPlayer GetNetworkPlayerReference(Player punPlayer)
        {
            PUN_CustomPlayer[] checkingPlayers = FindObjectsOfType<PUN_CustomPlayer>();
            foreach (PUN_CustomPlayer checkingPlayer in checkingPlayers)
            {
                if (checkingPlayer.ActorNumber == punPlayer.ActorNumber)
                    return checkingPlayer;
            }
            return null;
        }


        #endregion

        #region RPCs

        [PunRPC]
        private void RPC_CreatePlayer()
        {
            this._networkPlayer = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NetworkPlayer"), transform.position, Quaternion.identity, 0);
        }

        [PunRPC]
        public void RPC_StartGame()
        {
            _isGameLoaded = true;
            PhotonNetwork.LoadLevel(_levelScene);
            _uiManager.ToggleRoomMenu();
            _uiManager.ToggleMultiplayerMenu();
            _uiManager.ToggleHUD();
            MatchStats = new Stats();
            _uiManager._uiTimer.timer = true;
            if (!PhotonNetwork.IsMasterClient)
                return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            //foreach (Player player in _localRoom.Players)
            //{
            //    Debug.Log($"Ich bin {player.NickName} Count {_localRoom.Players.Length}");
            //}
        }

        [PunRPC]
        public void SpawnUnit()
        {

        }

        //[PunRPC]
        //public void RPC_AddPlayerEntry(Player newPlayer)
        //{
        //    //PUN_PlayerlistEntry newLine = Instantiate(_uiManager?._playerEntryPrefab, _uiManager?._PlayerList);
        //    GameManager.MasterManager.AddPlayer(newPlayer);
        //    //newLine.UpdatePlayerlistEntry(newPlayer);
        //    //_uiManager._PlayerName.text = newPlayer.NickName;
        //    //_playerListEntries.Add(newPlayer, newLine.gameObject);
        //}

        [PunRPC]
        public void RPC_RemovePlayerEntry(Player leavingPlayer)
        {
            RemovePlayerEntry(leavingPlayer);
        }

        [PunRPC]
        public void RPC_SetCrystalViews(Player[] players)
        {
            Crystal randomCrystal;
            foreach (Player player in players)
            {
                randomCrystal = GameManager.MasterManager.bases[Random.Range(0, GameManager.MasterManager.bases.Count)];
                randomCrystal.SetCrystalView(player);
                GameManager.MasterManager.bases.Remove(randomCrystal);
            }
            GameManager.MasterManager.StartInitCrystals();
        }

        [PunRPC]
        public void RPC_SetUnitView(Player player, Unit unit)
        {
            //unit._view.TransferOwnership(player);
        }

        #endregion


        #endregion
    }
}
