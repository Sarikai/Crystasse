using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype
{
    public class UI_Manager : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        public static UI_Manager uiManager;
        [SerializeField]
        private Transform _serverList;
        public Transform GetServerList { get { return _serverList; } }
        public UI_ServerlistContentLine _serverlistContentLine;


        //Menus to toggle
        public GameObject _MainMenu;
        public GameObject _NetworkMenu;
        public GameObject _WaitingMenu;
        public GameObject _LobbyMenu;
        public GameObject _RoomMenu;
        public GameObject _CreateRoomMenu;

        //Buttons
        public GameObject _NewGameButton;
        public GameObject _JoinServerButton;
        public GameObject _ServerToMainMenuButton;
        public GameObject _ServerNewRoomButton;
        public GameObject _LobbyToMainMenuButton;
        public GameObject _LobbyToServerButton;
        public GameObject _CancelRoomCreationButton;
        public GameObject _CreateRoomButton;
        //public GameObject _AbortButton;
        public GameObject _ExitAppButton;
        public KeyCode _EscapeButton = KeyCode.Escape;

        //InputFields
        public TextMeshProUGUI _RoomNameInput;
        public TextMeshProUGUI _PlayerNameInput;
        public TextMeshProUGUI _RoomName;

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
            if (!_NetworkMenu.activeSelf)
                uiManager.Toggle(_NetworkMenu);
            uiManager.Toggle(_CreateRoomMenu);

        }

        public void OnJoinServerButtonClicked()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            uiManager.Toggle(_MainMenu);
            if (!_NetworkMenu.activeSelf)
                uiManager.Toggle(_NetworkMenu);
            uiManager.Toggle(_LobbyMenu);
        }

        public void OnCreateRoomButtonClicked()
        {
            uiManager.Toggle(_LobbyMenu);
            uiManager.Toggle(_CreateRoomMenu);
        }
        public void Toggle(GameObject objectToToggle)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }

        public void OnExitGameClicked()
        {
            Debug.Log($"Game closing");
        }

        public void UpdateServerList()
        {

        }

        public void Server_OnBackToMainClicked()
        {
            Toggle(_LobbyMenu);
            Toggle(_MainMenu);
        }

        public void Room_OnBackToMainClicked()
        {
            Toggle(_RoomMenu);
            Toggle(_MainMenu);
        }

        public void Room_OnBackToServerClicked()
        {
            Toggle(_RoomMenu);
            Toggle(_LobbyMenu);
        }
        #endregion
    }
}
