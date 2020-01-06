using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class Stats
{


    #region Variables / Properties

    public uint spawnedUnits;
    public uint destroyedUnits;

    //private readonly string _filename = "Stats.dat";
    public Dictionary<int, string> Matches = new Dictionary<int, string>();

    #endregion

    #region Methods


    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.O))
    //    {
    //        Debug.Log("Saving");
    //        //UnityEngine.Random.Range(0, 6);
    //        Stats newStat = new Stats()
    //        {

    //            destroyedUnits = (uint)UnityEngine.Random.Range(0, 6),
    //            spawnedUnits = (uint)UnityEngine.Random.Range(0, 6),
    //        };
    //        Match.SaveMatch(newStat);
    //        GameManager.MasterManager._RunningSessionStats.AutoSaveStats();
    //    };

    //    if (Input.GetKey(KeyCode.L))
    //    {
    //        GameManager.MasterManager._RunningSessionStats.Matches = new Dictionary<int, string>();
    //        GameManager.MasterManager._RunningSessionStats.LoadStats();
    //        if (Matches != null && Matches.Count > 0)
    //        {
    //            foreach (KeyValuePair<int, string> matchPath in Matches)
    //            {
    //                Match.LoadMatch(matchPath.Value);
    //            }
    //        }

    //    }

    //}

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
        s = GameManager.MasterManager._RunningSessionStats;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/stats.dat");
        bf.Serialize(file, s);
        file.Close();

        BinaryFormatter clearbf = new BinaryFormatter();
        FileStream clearfile = File.Create(Application.persistentDataPath + "/stats.dat");
        clearbf.Serialize(clearfile, s);
        clearfile.Close();
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
            Matches = s.Matches;
        }
    }

    #endregion
}