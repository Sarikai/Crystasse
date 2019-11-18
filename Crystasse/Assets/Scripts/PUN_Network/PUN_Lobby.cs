using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PUN_Network
{
    public class PUN_Lobby : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        //Variables
        private List<RoomInfo> _rooms;

        //Properties
        public List<RoomInfo> Rooms { get { return _rooms; } set { _rooms = value; } }

        #endregion

        #region Methods

        #endregion
    }
}
