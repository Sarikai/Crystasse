using Boo.Lang;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using New;

namespace Prototype
{
    public class PUN_Lobby : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        public static PUN_Lobby Lobby;
        //private RoomInfo[] _rooms;
        private System.Collections.Generic.List<RoomInfo> _rooms;
        //private UI_Manager _uiManager;

        //Nachfolgendes entfernen/ersetzen mit UI_Manager
        public GameObject battleButton;
        public GameObject cancelButton;
        public ServerSetting mySetting;

        #endregion

        #region Methods

        private void Awake()
        {
            if (PUN_Lobby.Lobby == null)
            {
                PUN_Lobby.Lobby = this;

            }
            else
            {
                if (PUN_Lobby.Lobby != this)
                {
                    Destroy(PUN_Lobby.Lobby.gameObject);
                    PUN_Lobby.Lobby = this;
                }

            }
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        private void Update()
        {
            OnRoomListUpdate(_rooms);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"Player connected to Photon-Master-Server");
            PhotonNetwork.AutomaticallySyncScene = true;
            //battleButton.SetActive(true);
            //UI_Manager.uiManager.Toggle(UI_Manager.uiManager?._NewGameButton);
        }

        public void OnNewGameButtonClicked()
        {
            PhotonNetwork.JoinRandomRoom();
            //UI_Manager.uiManager.Toggle(UI_Manager.uiManager?._NewGameButton);
            //UI_Manager.uiManager.Toggle(UI_Manager.uiManager?._AbortButton);
            //battleButton.SetActive(false);
            //cancelButton.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Tried to join failed, no games available");
            CreateRoom();
        }

        void CreateRoom()
        {
            int randomRoomName = Random.Range(0, 1000);
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)ServerSetting.multiplayerSetting.maxPlayers };
            PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Tried to create room failed, name must already exist");
            CreateRoom();
        }

        public void OnCancelButtonClicked()
        {
            battleButton.SetActive(true);
            cancelButton.SetActive(false);
            PhotonNetwork.LeaveRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Connected to Room");
        }

        public override void OnRoomListUpdate(System.Collections.Generic.List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            if (_rooms != null)
            {
                foreach (RoomInfo room in roomList)
                {
                    UI_ServerlistContentLine newLine = Instantiate(UI_Manager.uiManager?._serverlistContentLine, UI_Manager.uiManager?.GetServerList);
                    newLine.UpdateContentLine(room);
                }
            }
        }
        #endregion
    }
}
