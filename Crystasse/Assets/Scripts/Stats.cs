using Photon.Pun;
using Photon.Realtime;
using Prototype;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Win32;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CustomUI
{
    [System.Serializable]
    public class Stats : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        uint spawnedUnits;
        uint destroyedUnits;

        private readonly string _filename = "Stats.dat";
        #endregion

        #region Methods

        public void IncrementSpawns()
        {
            spawnedUnits++;
        }

        public void ResetSpawns()
        {
            spawnedUnits = 0;
        }

        public void IncrementKills()
        {
            destroyedUnits++;
        }

        public void ResetKills()
        {
            destroyedUnits = 0;
        }


        public void AutoSaveStats()
        {
            Stats s = new Stats();
            s = GameManager.gameManager._RunningSessionStats;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/stats.dat");
            bf.Serialize(file, s);
            file.Close();
        }

        public void LoadStats()
        {
            if (File.Exists(Application.persistentDataPath + "/stats.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/stats.dat", FileMode.Open);
                Stats s = (Stats)bf.Deserialize(file);
                file.Close();

                spawnedUnits = s.spawnedUnits;
                destroyedUnits = s.destroyedUnits;
            }
        }

        #endregion
    }
}
