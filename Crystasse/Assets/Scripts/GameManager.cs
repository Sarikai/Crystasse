using CustomUI;
using Photon.Pun;
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


    //Properties
    public UI_Manager UIManager { get { return _uiManager; } set { _uiManager = value; } }
    public PUN_NetworkManager NetworkManager { get { return _networkManager; } set { _networkManager = value; } }
    public PhotonView MainView { get { return _mainView; } set { _mainView = value; } }

    #endregion

    #region Methods

    private void Awake()
    {
        GameManagerSingleton();
        _uiManager = GetComponent<UI_Manager>();
        _networkManager = GetComponent<PUN_NetworkManager>();
        _mainView = GetComponent<PhotonView>();
    }

    protected void GameManagerSingleton()
    {
        if (GameManager.MasterManager == null)
        {
            GameManager.MasterManager = this;
        }
        else
        {
            if (GameManager.MasterManager != this)
            {
                Destroy(GameManager.MasterManager.gameObject);
                GameManager.MasterManager = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion
}
