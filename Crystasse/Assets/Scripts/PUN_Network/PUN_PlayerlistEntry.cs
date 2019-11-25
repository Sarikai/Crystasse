using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Michsky.UI.ModernUIPack;

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
        int _myID = 1;

        [SerializeField]
        UIGradient _entryGradient;
        [SerializeField]
        Gradient _playerReadyGradient;
        [SerializeField]
        Gradient _playerNotReadyGradient;

        #endregion

        #region Methods

        private void Awake()
        {
            _entryGradient = this.GetComponent<UIGradient>();
        }

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

        public void OnPlayerEntryClicked()
        {
            if (_playerID.text == _myID.ToString())
            {
                _playerReady = !_playerReady;
            }
            switch (_playerReady)
            {
                case true: _entryGradient.EffectGradient = _playerReadyGradient; break;
                case false: _entryGradient.EffectGradient = _playerNotReadyGradient; break;

            }

        }

        #endregion
    }
}
