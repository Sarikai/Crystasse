using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Stats : MonoBehaviour
{


    #region Variables / Properties

    public uint spawnedUnits;
    public uint destroyedUnits;

    //private readonly string _filename = "Stats.txt";
    public Dictionary<int, string> Matches = new Dictionary<int, string>();

    #endregion

    #region Methods


    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            //UnityEngine.Random.Range(0, 6);
            Stats newStat = new Stats()
            {
                destroyedUnits = (uint)UnityEngine.Random.Range(0, 6),
                spawnedUnits = (uint)UnityEngine.Random.Range(0, 6),
            };
            Match.SaveMatch(newStat);
            AutoSaveStats();
        };

        if (Input.GetKey(KeyCode.L))
        {
            Matches = new Dictionary<int, string>();
            LoadStats();
            if (Matches != null && Matches.Count > 0)
            {
                foreach (KeyValuePair<int, string> matchPath in Matches)
                {
                    Match.LoadMatch(matchPath.Value);
                }
            }

        }

    }

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
        FileStream file = File.Create(Application.persistentDataPath + "/stats.txt");
        bf.Serialize(file, s);
        file.Close();
    }

    public void LoadStats()
    {
        if (File.Exists(Application.persistentDataPath + "/stats.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/stats.txt", FileMode.Open);
            Stats s = (Stats)bf.Deserialize(file);
            file.Close();

            spawnedUnits = s.spawnedUnits;
            destroyedUnits = s.destroyedUnits;
            Matches = s.Matches;
        }
    }

    #endregion
}