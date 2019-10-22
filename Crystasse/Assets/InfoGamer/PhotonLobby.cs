using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Prototype
{
    public class PhotonLobby : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties
        public static PhotonLobby lobby;
        RoomInfo[] rooms;

        public GameObject battleButton;
        public GameObject cancelButton;
        #endregion

        #region Methods
        private void Awake()
        {
            lobby = this;
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {

            Debug.Log("Player connected to Photon Master-Server");
            PhotonNetwork.AutomaticallySyncScene = true;
            battleButton.SetActive(true);
        }

        public void OnBattleButtonClicked()
        {
            PhotonNetwork.JoinRandomRoom();
            battleButton.SetActive(false);
            cancelButton.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Tried to join failed, no games available");
            CreateRoom();
        }

        void CreateRoom()
        {
            int randomRoomName = Random.Range(0, 1000);
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSetting.maxPlayers };
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
        #endregion
    }
}
