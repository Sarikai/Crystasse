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

    [RequireComponent(typeof(PhotonView))]
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
        string _myID = "";

        [SerializeField]
        UIGradient _entryGradient;
        [SerializeField]
        Gradient _playerReadyGradient;
        [SerializeField]
        Gradient _playerNotReadyGradient;
        [SerializeField]
        public PhotonView _entryView;

        #endregion

        #region Methods

        private void Awake()
        {
            _entryGradient = this.GetComponent<UIGradient>();
            //if (int.TryParse(GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId, out int userID))
            //{
            //    _myID = userID;
            //    Debug.Log($"Parse ok");
            //}
        }

        public virtual void UpdatePlayerlistEntry()
        {
            _myID = GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId;
            _playerID.text = "0";
            _playerName.text = "I am a testplayer";
            _playerReady = false;
            _playerTeam = 0;
        }

        public void UpdatePlayerlistEntry(Player player)
        {
            _myID = GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId;
            _entryView = GetComponent<PhotonView>();
            Debug.Log($"{player.ActorNumber}");
            _entryView.ViewID = 999 + player.ActorNumber;
            _entryView.TransferOwnership(player);
            ChangeEntryColor();
            //Debug.Log($"Update Entry ID: {player.UserId}");
            _playerID.text = player.UserId;
            _playerName.text = player.NickName;
            _playerReady = false;
            _playerTeam = 0;
        }

        public void UpdatePlayerlistEntry(int playerID, string playerName, int players, int maxPlayers)
        {

        }

        public void OnPlayerEntryClicked()
        {
            Debug.Log($"Entry Clicked. PlayerIdText: {_playerID.text} My Id: {_myID.ToString()} EntryViewId:{_entryView.ViewID}");
            //if (_playerID.text == _myID.ToString())
            //{
            //    Debug.Log($"Equals");
            //    _playerReady = !_playerReady;
            //}
            //switch (_playerReady)
            //{
            //    case true: _entryGradient.EffectGradient = _playerReadyGradient; break;
            //    case false: _entryGradient.EffectGradient = _playerNotReadyGradient; break;
            //}
            if (_entryView.IsMine)
            {
                _playerReady = !_playerReady;
                //ChangeEntryColor();
                photonView.RPC("RPC_ChangeReady", RpcTarget.AllBufferedViaServer);
            }

        }

        private void ChangeEntryColor()
        {
            switch (_playerReady)
            {
                case true: _entryGradient.EffectGradient = _playerReadyGradient; break;
                case false: _entryGradient.EffectGradient = _playerNotReadyGradient; break;
            }
        }

        [PunRPC]
        public void RPC_ChangeReady()
        {
            ChangeEntryColor();
            //switch (_playerReady)
            //{
            //    case true: _entryGradient.EffectGradient = _playerReadyGradient; break;
            //    case false: _entryGradient.EffectGradient = _playerNotReadyGradient; break;
            //}

        }


        #endregion
    }
}
