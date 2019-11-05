using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Michsky.UI.ModernUIPack;


namespace Prototype
{
    [RequireComponent(typeof(PhotonView))]
    public class UI_ServerlistContentLine : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties
        [SerializeField]
        TextMeshProUGUI _serverID,
        _serverName,
         _serverConnectedPlayer,
         _serverMaxPlayer;

        PhotonView _serverlinePV;

        UIGradient iGradient;
        #endregion

        #region Methods

        private void Awake()
        {
            _serverlinePV = GetComponent<PhotonView>();
        }

        public void UpdateContentLine(int serverID, String serverName, int serverConnectedPlayers, int serverMaxPlayer)
        {
            _serverID.text = serverID.ToString();
            _serverName.text = serverName;
            _serverConnectedPlayer.text = serverConnectedPlayers.ToString();
            _serverMaxPlayer.text = serverMaxPlayer.ToString();
        }

        public void UpdateContentLine(RoomInfo roomInfo)
        {
            _serverID.text = roomInfo.ID.ToString();
            _serverName.text = roomInfo.Name;
            _serverConnectedPlayer.text = roomInfo.PlayerCount.ToString();
            _serverMaxPlayer.text = roomInfo.MaxPlayers.ToString();
            Debug.Log($"Update done!");
        }
        #endregion
    }
}

//private void Start()
//{
//    //iGradient = GetComponent<UIGradient>();
//    //GradientColorKey[] colorKeys = this.GetComponent<UIGradient>().EffectGradient.colorKeys;
//    //GradientAlphaKey[] alphaKeys = this.GetComponent<UIGradient>().EffectGradient.alphaKeys;
//    //foreach (GradientColorKey c in colorKeys)
//    //{
//    //    foreach (GradientAlphaKey a in alphaKeys)
//    //    {
//    //        Debug.Log($"{c.color}, {a.alpha}");
//    //    }
//    //}

//}