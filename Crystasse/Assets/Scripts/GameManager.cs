using CustomUI;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using PUN_Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables / Properties

    //Variables
    public static GameManager MasterManager;
    [SerializeField]
    private UI_Manager _uiManager;
    [SerializeField]
    private PUN_NetworkManager _networkManager;
    [SerializeField]
    private InputManager _inputManager;
    [SerializeField]
    private SoundManager _soundManager;

    public List<Crystal> bases;
    public Dictionary<byte, Player> teamToPlayer = new Dictionary<byte, Player>();
    public Crystal[] crystals;

    [SerializeField]
    Material[] crystalMaterials;

    public List<GameObject> ObjectsToDestroy { get; private set; }
    [SerializeField]
    private bool doUpdate = false;
    public Stats _RunningSessionStats;
    public List<UI_StatEntry> _StatEntries;

    public string _unitPrefabLocation = "Unit";
    public string _crystalPrefabLocation;
    //Properties
    public UI_Manager UIManager { get { return _uiManager; } set { _uiManager = value; } }
    public PUN_NetworkManager NetworkManager { get { return _networkManager; } set { _networkManager = value; } }

    public InputManager InputManager { get { return _inputManager; } set { _inputManager = value; } }
    public SoundManager SoundManager { get { return _soundManager; } set { _soundManager = value; } }

    public bool DoUpdate { get => doUpdate; set => doUpdate = value; }
    public Material[] CrystalMaterials { get => crystalMaterials; set => crystalMaterials = value; }

    #endregion

    #region Methods

    private void Awake()
    {
        ObjectsToDestroy = new List<GameObject>();
        GameManagerSingleton();

        for(byte i = 0; i < byte.MaxValue; i++)
            teamToPlayer.Add(i, null);

        _RunningSessionStats = new Stats();
        _RunningSessionStats.Matches = new Dictionary<int, string>();
        SoundManager.MenuMusic();
    }



    public void AddPlayer(Player player)
    {
        for(byte i = 1; i < teamToPlayer.Count; i++)
            if(teamToPlayer[i] == null)
            {
                teamToPlayer[i] = player;
                GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID = i;
                return;
            }
    }

    protected void GameManagerSingleton()
    {
        if(GameManager.MasterManager == null)
        {
            GameManager.MasterManager = this;
        }
        else
        {
            if(GameManager.MasterManager != this)
            {
                Destroy(GameManager.MasterManager.gameObject);
                GameManager.MasterManager = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(NetworkManager._isGameLoaded && Input.GetKeyDown(UIManager._escapeKey))
        {
            UIManager.ToggleIngameMenu();
        }

        //if (Input.GetKey(KeyCode.O))
        //{
        //    Debug.Log("Saving");
        //    //UnityEngine.Random.Range(0, 6);
        //    Stats newStat = new Stats()
        //    {
        //        destroyedUnits = (uint)UnityEngine.Random.Range(0, 6),
        //        spawnedUnits = (uint)UnityEngine.Random.Range(0, 6),
        //    };
        //    Match.SaveMatch(newStat);
        //    GameManager.MasterManager._RunningSessionStats.AutoSaveStats();
        //};


        //TODO: [DONE] get this variable true!!!
        if(DoUpdate)
        {
            StateMachine.Update();

            foreach(var c in crystals)
                c.UpdateCrystal();
        }



    }

    public void LoadStats()
    {
        GameManager.MasterManager._RunningSessionStats.Matches = new Dictionary<int, string>();
        GameManager.MasterManager._RunningSessionStats.LoadStats();
        if (GameManager.MasterManager._RunningSessionStats.Matches != null && GameManager.MasterManager._RunningSessionStats.Matches.Count > 0)
        {
            foreach (KeyValuePair<int, string> matchPath in GameManager.MasterManager._RunningSessionStats.Matches)
            {
                Match m = Match.LoadMatch(matchPath.Value);
                if (m != null)
                {
                    UI_StatEntry matchStats = Instantiate(GameManager.MasterManager.UIManager._matchLinePrefab, GameManager.MasterManager.UIManager._MatchList.position, Quaternion.identity, GameManager.MasterManager.UIManager._MatchList);
                    //UI_StatEntry matchStats = GameManager.MasterManager.UIManager.InstantiateLine();
                    matchStats.UpdateEntry(m);
                    GameManager.MasterManager.NetworkManager._matchEntries.Add(m, matchStats.gameObject);
                }
            }
        }
    }

    #endregion
}
