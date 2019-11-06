using CustomUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PUN_Network
{
    [RequireComponent(typeof(PUN_Lobby), typeof(PUN_Room), typeof(PUN_RoomSettings))]
    public class PUN_NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        PUN_Lobby _localLobby;
        PUN_Room _localRoom;
        PUN_RoomSettings _defaultRoomSettings;
        UI_Manager _uiManager;


        public PUN_Room GetRoom { get { return _localRoom; } }

        #endregion

        #region Methods

        private void Awake()
        {
            _localLobby = GetComponent<PUN_Lobby>();
            _localRoom = GetComponent<PUN_Room>();
            _defaultRoomSettings = GetComponent<PUN_RoomSettings>();
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }



        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
        }

        #region LobbyMethods



        #endregion

        #region RoomMethods

        /// <summary>
        /// Creates a random room. No setup needed.
        /// </summary>
        private void CreateRoom()
        {
            int randomRoomNumber = Random.Range(0, 1000);
            RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)_defaultRoomSettings.MaxPlayers, PublishUserId = true };
            PhotonNetwork.CreateRoom($"Room: {randomRoomNumber}", roomOptions);
        }

        /// <summary>
        /// Creates a custom Room with just the name given.
        /// </summary>
        /// <param name="roomName">The name that the host chose.</param>
        private void CreateRoom(string roomName)
        {

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

        #endregion


        #endregion
    }
}
