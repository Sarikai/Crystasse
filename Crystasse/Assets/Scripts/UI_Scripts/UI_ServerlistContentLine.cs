using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Michsky.UI.ModernUIPack;

namespace Prototype
{
    public class UI_ServerlistContentLine : MonoBehaviour
    {
        #region Variables / Properties
        TextMeshProUGUI _serverID;
        TextMeshProUGUI _serverName;
        TextMeshProUGUI _serverConnectedPlayer;
        TextMeshProUGUI _serverMaxPlayer;

        UIGradient iGradient;
        #endregion

        #region Methods
        private void Start()
        {
            iGradient = GetComponent<UIGradient>();
            GradientColorKey[] colorKeys = this.GetComponent<UIGradient>().EffectGradient.colorKeys;
            GradientAlphaKey[] alphaKeys = this.GetComponent<UIGradient>().EffectGradient.alphaKeys;
            foreach (GradientColorKey c in colorKeys)
            {
                foreach (GradientAlphaKey a in alphaKeys)
                {
                    Debug.Log($"{c.color}, {a.alpha}");
                }
            }

        }
        #endregion
    }
}
