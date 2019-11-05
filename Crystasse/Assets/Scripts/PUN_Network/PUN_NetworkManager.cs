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

        #endregion

        #region Methods

        private void Awake()
        {
            _localLobby = GetComponent<PUN_Lobby>();
            _localRoom = GetComponent<PUN_Room>();
            _defaultRoomSettings = GetComponent<PUN_RoomSettings>();
        }

        #region LobbyMethods



        #endregion

        #region RoomMethods



        #endregion


        #endregion
    }
}
