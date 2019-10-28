using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype
{
    public class UI_Manager : MonoBehaviour
    {
        #region Variables / Properties
        private Transform _serverList;
        public UI_ServerlistContentLine _serverlistContentLine;
        public Transform GetServerList { get { return _serverList; } }
        public static UI_Manager uiManager;

        [SerializeField]
        public GameObject _NewGameButton;
        [SerializeField]
        public GameObject _JoinServerButton;
        //[SerializeField]
        //public GameObject _AbortButton;
        [SerializeField]
        public GameObject _ExitButton;

        public GameObject _MainMenu;
        public GameObject _NetworkMenu;
        public GameObject _WaitingMenu;
        public GameObject _LobbyMenu;
        public GameObject _RoomMenu;

        #endregion

        #region Methods

        private void Awake()
        {
            if (UI_Manager.uiManager == null)
            {
                UI_Manager.uiManager = this;

            }
            else
            {
                if (UI_Manager.uiManager != this)
                {
                    Destroy(UI_Manager.uiManager.gameObject);
                    UI_Manager.uiManager = this;
                }

            }
            DontDestroyOnLoad(this.gameObject);
        }

        public void OnNewGameButtonClicked()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            uiManager.Toggle(_MainMenu);
            uiManager.Toggle(_NetworkMenu);
            uiManager.Toggle(_RoomMenu);
        }

        public void OnJoinServerButtonClicked()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            uiManager.Toggle(_MainMenu);
            uiManager.Toggle(_NetworkMenu);
            uiManager.Toggle(_LobbyMenu);
        }
        public void Toggle(GameObject objectToToggle)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }

        #endregion
    }
}
