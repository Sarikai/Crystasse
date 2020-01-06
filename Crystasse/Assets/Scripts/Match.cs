
using CustomUI;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Mathematics;
using UnityEngine;

public class Match : MonoBehaviour
{

    #region Variables / Properties

    public uint spawnedUnits;
    public uint destroyedUnits;
    public string date;

    private readonly string _filename = "Stats.txt";
    List<Match> _matchLines = new List<Match>();

    #endregion

    #region Methods


    public void SaveMatch(Stats matchStats)
    {
        Match m = new Match();
        m.destroyedUnits = matchStats.destroyedUnits;
        m.spawnedUnits = matchStats.spawnedUnits;

        BinaryFormatter bf = new BinaryFormatter();
        //string fileName = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}";
        string fileName = $"{DateTime.Now.ToString()}";
        if (!File.Exists($"{Application.persistentDataPath}/{fileName}.txt"))
        {
            GameManager.MasterManager._RunningSessionStats.Matches.Add(GameManager.MasterManager._RunningSessionStats.Matches.Count + 1, fileName);
            FileStream file = File.Create($"{Application.persistentDataPath}/{fileName}.txt");
            bf.Serialize(file, m);
            file.Close();
        }
    }

    public void LoadMatch(String path)
    {
        if (File.Exists($"{Application.persistentDataPath}/{path}.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open($"{Application.persistentDataPath}/{path}.txt", FileMode.Open);
            Match m = (Match)bf.Deserialize(file);
            file.Close();

            UI_Stats matchStats = new UI_Stats()
            {
                spawnedUnits = m.spawnedUnits,
                destroyedUnits = m.destroyedUnits,
                date = File.GetCreationTime($"{Application.persistentDataPath}/{path}.txt").ToString()
            };

            _matchLines.Add(Instantiate(GameManager.MasterManager.UIManager._matchLinePrefab, GameManager.MasterManager.UIManager._MatchList.transform.position, Quaternion.identity));
        }
    }

    #endregion

}
