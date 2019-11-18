using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class GameManager : MonoBehaviour
    {
        #region Variables / Properties
        public static GameManager gameManager;
        public Stats _RunningSessionStats;
        #endregion

        #region Methods
        private void Awake()
        {
            if (GameManager.gameManager == null)
            {
                GameManager.gameManager = this;
            }
            else
            {
                if (GameManager.gameManager != this)
                {
                    Destroy(this.gameObject);
                }
            }
            DontDestroyOnLoad(this.gameObject);

            _RunningSessionStats = new Stats();
        }
        #endregion
    }
}
