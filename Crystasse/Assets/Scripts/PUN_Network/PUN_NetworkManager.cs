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
        public static PUN_NetworkManager NetworkManager;

        PUN_Lobby _localLobby;
        PUN_Room _localRoom;
        PUN_RoomSettings _defaultRoomSettings;
        UI_Manager _uiManager;

        #endregion

        #region Methods

        private void Awake()
        {
            NetworkManagerSingleton();

            _localLobby = GetComponent<PUN_Lobby>();
            _localRoom = GetComponent<PUN_Room>();
            _defaultRoomSettings = GetComponent<PUN_RoomSettings>();
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        protected void NetworkManagerSingleton()
        {
            if (PUN_NetworkManager.NetworkManager == null)
            {
                PUN_NetworkManager.NetworkManager = this;
            }
            else
            {
                if (PUN_NetworkManager.NetworkManager != this)
                {
                    Destroy(PUN_NetworkManager.NetworkManager.gameObject);
                    PUN_NetworkManager.NetworkManager = this;
                }
            }
            DontDestroyOnLoad(this.gameObject);
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
            RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)_defaultRoomSettings.MaxPlayers };
            PhotonNetwork.CreateRoom($"Room: {randomRoomNumber}", roomOptions);
        }

        /// <summary>
        /// Creates a custom Room with just the name given.
        /// </summary>
        /// <param name="roomName">The name that the host chose.</param>
        private void CreateRoom(string roomName)
        {

        }

        #endregion


        #endregion
    }
}
