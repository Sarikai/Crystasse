using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class UI_Manager : MonoBehaviour
    {
        #region Variables / Properties

        #endregion

        #region Methods

        public void Toggle(GameObject objectToToggle)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }

        #endregion
    }
}
