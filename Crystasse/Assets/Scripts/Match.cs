
using CustomUI;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class Match
{

    #region Variables / Properties

    public uint spawnedUnits;
    public uint destroyedUnits;
    public string date;
    public string duration;

    private readonly string _filename = "Stats.dat";
    #endregion

    #region Methods

    public static void SaveMatch(Stats matchStats)
    {
        Match m = new Match();
        m.destroyedUnits = matchStats.destroyedUnits;
        m.spawnedUnits = matchStats.spawnedUnits;
        m.date = DateTime.Today.ToString();
        //m.duration = GameManager.MasterManager.UIManager._uiTimer.TimeFormatter();
        m.duration = GameManager.MasterManager.UIManager._uiTimer.TimeFormatter(UnityEngine.Random.Range(0f, 3600f));

        BinaryFormatter bf = new BinaryFormatter();
        string fileName = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}-{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}";
        Debug.Log(DateTime.Now.ToString());
        Debug.Log($"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}-{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}");
        //string fileName = $"{DateTime.Now.ToString()}";
        //Debug.Log(!File.Exists($"{Application.persistentDataPath}/{fileName}.dat"));
        if (!File.Exists($"{Application.persistentDataPath}/{fileName}.dat"))
        {
            GameManager.MasterManager._RunningSessionStats.Matches.Add(GameManager.MasterManager._RunningSessionStats.Matches.Count + 1, fileName);
            FileStream file = File.Create($"{Application.persistentDataPath}/{fileName}.dat");
            bf.Serialize(file, m);
            file.Close();
            Debug.Log($"Match saved to {Application.persistentDataPath}/{fileName}.dat");
        }
    }

    public static Match LoadMatch(String path)
    {
        if (File.Exists($"{Application.persistentDataPath}/{path}.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open($"{Application.persistentDataPath}/{path}.dat", FileMode.Open);
            Match m = (Match)bf.Deserialize(file);
            file.Close();

            //UI_StatEntry matchStats = Instantiate(GameManager.MasterManager.UIManager._matchLinePrefab, GameManager.MasterManager.UIManager._MatchList.transform.position, Quaternion.identity);

            //UI_StatEntry matchStats = GameManager.MasterManager.UIManager.InstantiateLine();
            //matchStats.UpdateEntry(m);
            //GameManager.MasterManager.NetworkManager._matchEntries.Add(m, matchStats.gameObject);

            //GameManager.MasterManager.UIManager.MatchList.Add(Instantiate(GameManager.MasterManager.UIManager._matchLinePrefab, GameManager.MasterManager.UIManager._MatchList.transform.position, Quaternion.identity));
            Debug.Log("Match returned");
            return m;
        }
        return null;
    }

    #endregion

}
