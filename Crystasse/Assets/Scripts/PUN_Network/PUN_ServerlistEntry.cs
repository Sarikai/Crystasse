using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace PUN_Network
{
    public class PUN_ServerlistEntry : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        [SerializeField]
        TextMeshProUGUI _serverID;
        [SerializeField]
        TextMeshProUGUI _serverName;
        [SerializeField]
        TextMeshProUGUI _serverPlayers;
        [SerializeField]
        TextMeshProUGUI _serverMaxPlayers;

        [SerializeField]
        Gradient _openRoomGradient;
        [SerializeField]
        Gradient _closedRoomGradient;

        #endregion

        #region Methods

        public virtual void UpdateServerlistEntry()
        {
            _serverID.text = "0";
            _serverName.text = "I am a Testserver";
            _serverPlayers.text = "1";
            _serverMaxPlayers.text = $"{PUN_NetworkManager.NetworkManager.GetRoom.GetRoomActiveSettings.MaxPlayers}";
        }

        public void UpdateServerlistEntry(RoomInfo roomInfo)
        {

        }

        public void UpdateServerlistEntry(int serverID, string serverName, int players, int maxPlayers)
        {

        }


        #endregion
    }
}
