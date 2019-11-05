using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Prototype
{
    public class PUN_Lobby : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        public static PUN_Lobby Lobby;
        private List<RoomInfo> _rooms;
        //private UI_Manager _uiManager;
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

        //private void Update()
        //{
        //    OnRoomListUpdate(_rooms);
        //}

        public override void OnConnectedToMaster()
        {
            Debug.Log($"Player connected to Photon-Master-Server");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.JoinLobby();
            //battleButton.SetActive(true);
            //UI_Manager.uiManager.Toggle(UI_Manager.uiManager?._NewGameButton);
        }

        public void OnNewGameButtonClicked()
        {
            PhotonNetwork.JoinRandomRoom();
            UI_Manager.uiManager.Toggle(UI_Manager.uiManager?._MainMenu);
            UI_Manager.uiManager.Toggle(UI_Manager.uiManager?._CreateRoomMenu);
            //battleButton.SetActive(false);
            //cancelButton.SetActive(true);
        }

        public void OnCreateRoomButtonClicked()
        {
            PhotonNetwork.JoinRoom(UI_Manager.uiManager._RoomNameInput.text);

            UI_Manager.uiManager.Toggle(UI_Manager.uiManager?._CreateRoomMenu);
            UI_Manager.uiManager.Toggle(UI_Manager.uiManager?._RoomMenu);
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Tried to join failed, room did not exist");
            CreateRoom(UI_Manager.uiManager._RoomNameInput.text);
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
            PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps, TypedLobby.Default);

        }

        void CreateRoom(string roomName)
        {
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)ServerSetting.multiplayerSetting.maxPlayers };
            PhotonNetwork.CreateRoom("Room: " + roomName, roomOps);
            UI_Manager.uiManager._RoomName.text = roomName;
            //RoomInfo newRoomInfo;
            //Room newRoom;
            //newRoom.SetCustomProperties(new Hashtable() { id})
            //_rooms.Add(new RoomInfo( ))
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Tried to create room failed, name must already exist");
            CreateRoom();
        }

        public void OnCancelButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Connected to Room");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log($"RoomUpdate called {_rooms != null}");
            //base.OnRoomListUpdate(roomList);
            if (roomList != null && roomList.Count > 0)
            {
                Debug.Log($"Rooms not null");
                foreach (RoomInfo roomInfo in roomList)
                {
                    Debug.Log($"{roomInfo.ToString()}");
                    UI_ServerlistContentLine newLine = Instantiate(UI_Manager.uiManager?._serverlistContentLine, UI_Manager.uiManager?.GetServerList);
                    newLine.UpdateContentLine(roomInfo);
                }
            }

        }
        #endregion
    }
}
