using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;


namespace CustomUI
{
    public class UI_StatEntry : MonoBehaviourPunCallbacks


    {
        #region Variables / Properties
        [SerializeField]
        TextMeshProUGUI spawned;
        [SerializeField]
        TextMeshProUGUI destroyed;
        [SerializeField]
        TextMeshProUGUI date;
        [SerializeField]
        TextMeshProUGUI duration;
        #endregion

        #region Methods
        public void UpdateEntry(Match match)
        {
            spawned.text = match.spawnedUnits.ToString();
            destroyed.text = match.destroyedUnits.ToString();
            date.text = match.date;
            duration.text = match.duration;
        }

        public void Remove()
        {
            Destroy(this);
        }
        #endregion
    }
}

