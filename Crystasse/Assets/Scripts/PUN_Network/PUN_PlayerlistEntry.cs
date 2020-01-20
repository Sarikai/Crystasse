using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI.Michsky.UI.ModernUIPack;

namespace PUN_Network
{

    [RequireComponent(typeof(PhotonView))]
    public class PUN_PlayerlistEntry : MonoBehaviourPunCallbacks, IPunObservable //TODO: [DONE] obeserveable for entry just sync the ready bool and let the update change the color
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
        [SerializeField]
        CustomDropdown _dropDown;

        public bool PlayerReady { get => _playerReady; set { _playerReady = value; ChangeEntryColor(); } }

        public int PlayerTeam { get => _playerTeam; set => _playerTeam = value; }



        #endregion

        #region Methods

        private void Awake()
        {
            //_entryView = GetComponent<PhotonView>();
            _entryGradient = this.GetComponent<UIGradient>();
            transform.SetParent(GameManager.MasterManager.UIManager._PlayerList);
            //if (int.TryParse(GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId, out int userID))
            //{
            //    _myID = userID;
            //    Debug.Log($"Parse ok");
            //}
        }

        //TODO: [DONE] Überarbeiten Player anlegen, 
        public virtual void UpdatePlayerlistEntry()
        {
            _myID = GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId;
            _playerID.text = "0";
            _playerName.text = "I am a testplayer";
            PlayerReady = false;
            PlayerTeam = 0;
        }

        public void UpdatePlayerlistEntry(PUN_CustomPlayer player)
        {
            _myID = GameManager.MasterManager.NetworkManager.GetLocalPlayer.UserId;
            _entryView = GetComponent<PhotonView>();
            ChangeEntryColor();
            _playerID.text = _myID;
            _playerName.text = player.NickName;
            PlayerReady = false;
            PlayerTeam = player.TeamID;
        }

        public void UpdatePlayerlistEntry(Player player)
        {
            _myID = PhotonNetwork.LocalPlayer.UserId;
            _entryView = GetComponent<PhotonView>();
            ChangeEntryColor();
            _playerID.text = _myID;
            _playerName.text = player.NickName;
            PlayerReady = false;

            //TODO: [DONE?] Setzen und removen der kvp bei Teamänderung
            foreach (KeyValuePair<byte, Player> kvp in GameManager.MasterManager.teamToPlayer)
            {
                if (kvp.Value == player)
                {
                    PlayerTeam = kvp.Key;
                }
            }
        }

        //TODO: [DONE] restrict access or calls to master client only or player only
        public void OnPlayerEntryClicked()
        {
            Debug.Log($"Entry Clicked. PlayerIdText: {_playerID.text} My Id: {_myID.ToString()} EntryViewId:{_entryView.ViewID}");

            if (_entryView.IsMine)
            {
                PlayerReady = !PlayerReady;
                //ChangeEntryColor();
            }
        }

        private void ChangeEntryColor()
        {
            if (PlayerReady)
                _entryGradient.EffectGradient = _playerReadyGradient;
            else
                _entryGradient.EffectGradient = _playerNotReadyGradient;
        }


        [PunRPC]
        public void RPC_ChangeIcon(int itemIndex)
        {
            if (Constants.UNIT_ICONS.Length > itemIndex)
            {
                _dropDown.selectedImage.sprite = Resources.Load<Sprite>(Constants.UNIT_ICONS[itemIndex]);
            }
            else
            {
                Debug.Log("UnitIcon does not exist!");
                _dropDown.selectedImage.sprite = Resources.Load<Sprite>(Constants.UNIT_ICONS[0]);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(PlayerReady);
                Debug.Log($"LocalClient sending ready {GetComponent<PhotonView>().ViewID}");
            }
            else
            {
                this.PlayerReady = (bool)stream.ReceiveNext();
                Debug.Log($"RemoteClient receiving ready { GetComponent<PhotonView>().ViewID}");
            }
        }

        #endregion
    }
}
