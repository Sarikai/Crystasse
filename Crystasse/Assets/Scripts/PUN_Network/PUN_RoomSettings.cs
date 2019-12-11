using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace PUN_Network
{
    public class PUN_RoomSettings : MonoBehaviour
    {
        #region Variables / Properties

        [SerializeField]
        private int timeToStart;
        [SerializeField]
        private int maxPlayers;
        [SerializeField]
        private bool delayedStart;
        [SerializeField]
        private bool privateRoom;
        [SerializeField]
        private bool autoStart;

        public int TimeToStart { get => timeToStart; set => timeToStart = value; }
        public int MaxPlayers { get => maxPlayers; set => maxPlayers = value; }
        public bool DelayedStart { get => delayedStart; set => delayedStart = value; }
        public bool PrivateRoom { get => privateRoom; set => privateRoom = value; }
        public bool AutoStart { get => autoStart; set => autoStart = value; }

        #endregion

        #region Methods

        #endregion
    }
}
