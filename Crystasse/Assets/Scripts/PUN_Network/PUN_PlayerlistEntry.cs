using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace PUN_Network
{
    public class PUN_PlayerlistEntry : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        [SerializeField]
        TextMeshProUGUI _playerID;
        [SerializeField]
        TextMeshProUGUI _playerName;
        [SerializeField]
        bool _playerReady;
        [SerializeField]
        int _playerTeam;

        [SerializeField]
        Gradient _playerReadyGradient;
        [SerializeField]
        Gradient _playerNotReadyGradient;

        #endregion

        #region Methods

        public virtual void UpdatePlayerlistEntry()
        {
            _playerID.text = "0";
            _playerName.text = "I am a testplayer";
            _playerReady = false;
            _playerTeam = 0;
        }

        public void UpdatePlayerlistEntry(Player player)
        {

        }

        public void UpdatePlayerlistEntry(int playerID, string playerName, int players, int maxPlayers)
        {
        
        }

        #endregion
    }
}
