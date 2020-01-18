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
    public class PUN_PlayerlistEntry : MonoBehaviourPunCallbacks, IPunObservable //TODO: obeserveable for entry just sync the ready bool and let the update change the color
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
            //_entryView = GetComponent<PhotonView>();
            _entryGradient = this.GetComponent<UIGradient>();
            //if (int.TryParse(GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId, out int userID))
            //{
            //    _myID = userID;
            //    Debug.Log($"Parse ok");
            //}
        }

        //TODO: Überarbeiten Player anlegen
        public virtual void UpdatePlayerlistEntry()
        {
            _myID = GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId;
            _playerID.text = "0";
            _playerName.text = "I am a testplayer";
            _playerReady = false;
            _playerTeam = 0;
        }

        //TODO: clearing maybe?
        public void UpdatePlayerlistEntry(PUN_CustomPlayer player)
        {
            _myID = GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId;
            _entryView = GetComponent<PhotonView>();
            //Debug.Log($"Actor Number in entry Update: {player.ActorNumber}");
            //_entryView.ViewID = 999 + player.ActorNumber;
            //_entryView.TransferOwnership(player.LocalPlayer);
            ChangeEntryColor();
            //Debug.Log($"Update Entry ID: {player.UserId}");
            //_playerID.text = player.LocalPlayer.UserId;
            _playerID.text = _myID;
            _playerName.text = player.NickName;
            _playerReady = false;
            _playerTeam = player.TeamID;
        }


        //TODO: remove?
        //public void UpdatePlayerlistEntry(int playerID, string playerName, int players, int maxPlayers)
        //{

        //}


        //TODO: restrict access or calls to master client only or player only
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
                ChangeEntryColor();
                //photonView.RPC("RPC_ChangeReady", RpcTarget.AllBufferedViaServer);
            }

        }

        private void ChangeEntryColor()
        {
            if (_playerReady)
                _entryGradient.EffectGradient = _playerReadyGradient;
            else
                _entryGradient.EffectGradient = _playerNotReadyGradient;
        }


        //[PunRPC]
        //public void RPC_ChangeReady()
        //{
        //    ChangeEntryColor();
        //    //switch (_playerReady)
        //    //{
        //    //    case true: _entryGradient.EffectGradient = _playerReadyGradient; break;
        //    //    case false: _entryGradient.EffectGradient = _playerNotReadyGradient; break;
        //    //}

        //}

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_playerReady);
                stream.SendNext(transform.parent);
                Debug.Log($"LocalClient {GetComponent<PhotonView>().ViewID}");
            }
            else
            {
                this._playerReady = (bool)stream.ReceiveNext();
                this.transform.parent = (Transform)stream.ReceiveNext();
                Debug.Log($"RemoteClient { GetComponent<PhotonView>().ViewID}");
            }
        }

        #endregion
    }
}
