﻿using CustomUI;
using Photon.Pun;
using Photon.Realtime;
using PUN_Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI_Manager), typeof(PUN_NetworkManager))]
public class GameManager : MonoBehaviour
{
    #region Variables / Properties

    //Variables
    public static GameManager MasterManager;
    private UI_Manager _uiManager;
    private PUN_NetworkManager _networkManager;
    private PhotonView _mainView;

    public Map map;
    public List<Crystal> bases;
    public Dictionary<byte, Player> teamToPlayer = new Dictionary<byte, Player>();
    public Crystal[] crystals;
    public List<GameObject> ObjectsToDestroy { get; private set; }
    [SerializeField]
    private bool doUpdate = false;

    //Properties
    public UI_Manager UIManager { get { return _uiManager; } set { _uiManager = value; } }
    public PUN_NetworkManager NetworkManager { get { return _networkManager; } set { _networkManager = value; } }
    public PhotonView MainView { get { return _mainView; } set { _mainView = value; } }

    #endregion

    #region Methods

    private void Awake()
    {
        ObjectsToDestroy = new List<GameObject>();
        GameManagerSingleton();
        _uiManager = GetComponent<UI_Manager>();
        _networkManager = GetComponent<PUN_NetworkManager>();
        _mainView = GetComponent<PhotonView>();

        for(byte i = 0; i < byte.MaxValue; i++)
            teamToPlayer.Add(i, null);
    }

    public void AddPlayer(Player player)
    {
        for(byte i = 1; i < teamToPlayer.Count; i++)
            if(teamToPlayer[i] == null)
            {
                teamToPlayer[i] = player;
                Debug.Log("Added player: " + player.NickName + " at teamID : " + i);
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

    public void LoadMap()
    {
        crystals = map.LoadMap(out bases);
        doUpdate = true;
    }

    public void StartInitCrystals()
    {
        if(crystals != null)
            foreach(var crystal in crystals)
                crystal.Init();
    }

    private void Update()
    {
        if(doUpdate)
        {
            StateMachine.Update();

            foreach(var c in crystals)
                c.UpdateCrystal();
        }
    }

    #endregion
}
