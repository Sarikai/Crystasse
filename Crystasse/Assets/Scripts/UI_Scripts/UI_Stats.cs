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
    public class UI_Stats : MonoBehaviourPunCallbacks


    {
        #region Variables / Properties
        [SerializeField]
        public uint spawnedUnits;
        [SerializeField]
        public uint destroyedUnits;
        [SerializeField]
        public string date;

        [SerializeField]
        TextMeshProUGUI spawned;
        [SerializeField]
        TextMeshProUGUI destroyed;
        [SerializeField]
        TextMeshProUGUI dateText;
        #endregion

        #region Methods
        public void UpdateLine(Match match)
        {
            spawned.text = match.spawnedUnits.ToString();
            destroyed.text = match.destroyedUnits.ToString();
            date = match.date;
        }
        #endregion
    }
}

