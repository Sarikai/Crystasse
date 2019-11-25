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
            _serverMaxPlayers.text = $"{GameManager.MasterManager.NetworkManager.GetRoom.GetRoomActiveSettings.MaxPlayers}";
        }

        public void UpdateServerlistEntry(RoomInfo roomInfo)
        {
            _serverID.text = roomInfo.ID.ToString();
            _serverName.text = roomInfo.Name;
            _serverPlayers.text = roomInfo.PlayerCount.ToString();
            _serverMaxPlayers.text = roomInfo.MaxPlayers.ToString();
        }

        public void UpdateServerlistEntry(int serverID, string serverName, int players, int maxPlayers)
        {
            _serverID.text = serverID.ToString();
            _serverName.text = serverName;
            _serverPlayers.text = players.ToString();
            _serverMaxPlayers.text = maxPlayers.ToString();
        }

        public void OnEntryClicked()
        {
            GameManager.MasterManager.NetworkManager.JoinRoom(_serverName.text);
        }

        #endregion
    }
}
