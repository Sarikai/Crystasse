using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class ServerSetting : MonoBehaviour
    {
        #region Variables / Properties
        public static ServerSetting multiplayerSetting;
        public bool delayStart;
        public int maxPlayers;

        public int menuScene;
        public int multiplayerScene;
        #endregion

        #region Methods
        private void Awake()
        {
            if (ServerSetting.multiplayerSetting == null)
            {
                ServerSetting.multiplayerSetting = this;
            }
            else
            {
                if (ServerSetting.multiplayerSetting != this)
                {
                    Destroy(this.gameObject);
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion
    }
}

