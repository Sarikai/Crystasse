using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


namespace CustomUI
{
    public class UI_Timer : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties
        private float time, m, s;
        private string hours, minutes, seconds;

        #endregion

        #region Methods
        private IEnumerator TimerCoroutine()
        {
            this.time = 0;
            while (GameManager.MasterManager.NetworkManager.startGame)
            {
                time += Time.deltaTime;
                hours = ((int)(time / 3600f)).ToString("00");
                m = time % 3600f;
                minutes = ((int)(m / 60f)).ToString("00");
                seconds = ((int)(s % 60f)).ToString("00");
                GameManager.MasterManager.UIManager._TimeRunning.text = ($"{hours}:{minutes}:{seconds}");
                Debug.Log($"{hours}:{minutes}:{seconds}");
            }
            yield break;
        }

        public void StartTimer()
        {

            StartCoroutine("TimerCoroutine");
            //GameManager.MasterManager.NetworkManager.photonView.RPC("RPC_StartTimer", RpcTarget.AllViaServer);
        }

        [PunRPC]
        public void RPC_StartTimer()
        {
            StartCoroutine("TimerCoroutine");
        }
        #endregion
    }
}
