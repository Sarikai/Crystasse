using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PUN_Network
{
    public class PUN_Room : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        //Variables
        PUN_RoomSettings _activeSettings;
        PUN_RoomSettings _defaultSettings;

        //Properties
        public PUN_RoomSettings GetRoomActiveSettings { get { return _activeSettings; } }
        #endregion

        #region Methods

        #endregion
    }
}
