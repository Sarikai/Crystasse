using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PUN_Network
{
    [RequireComponent(typeof(PUN_RoomSettings))]
    public class PUN_Room : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        //Variables
        Room _reliantPhotonRoom;
        PUN_RoomSettings _activeSettings;
        [SerializeField]
        PUN_RoomSettings _defaultSettings;
        Player[] _photonPlayers;
        public int PlayersInRoom;
        public int MyNumberInRoom;

        //Properties
        public PUN_RoomSettings GetRoomActiveSettings { get { return _activeSettings; } }
        public Player[] Players { get { return _photonPlayers; } set { _photonPlayers = value; } }

        public Room Room { get { return _reliantPhotonRoom; } set { _reliantPhotonRoom = value; } }

        #endregion

        #region Methods

        private void Awake()
        {
            _activeSettings = _defaultSettings;
        }

        public Player[] UpdatePlayers()
        {
            return _photonPlayers = PhotonNetwork.PlayerList;
        }

        #endregion
    }
}
