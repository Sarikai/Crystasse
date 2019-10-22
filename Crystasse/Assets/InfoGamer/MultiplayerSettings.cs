using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class MultiplayerSettings : MonoBehaviour
    {
        #region Variables / Properties
        public static MultiplayerSettings multiplayerSetting;
        public bool delayStart;
        public int maxPlayers;

        public int menuScene;
        public int multiplayerScene;
        #endregion

        #region Methods
        private void Awake()
        {
            if (MultiplayerSettings.multiplayerSetting == null)
            {
                MultiplayerSettings.multiplayerSetting = this;
            }
            else
            {
                if (MultiplayerSettings.multiplayerSetting != this)
                {
                    Destroy(this.gameObject);
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }

        #endregion
    }
}
