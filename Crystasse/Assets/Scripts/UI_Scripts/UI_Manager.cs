using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;


namespace CustomUI
{
    public class UI_Manager : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        public static UI_Manager Manager;

        //MenusToToggle
        [Header("Menus")]
        public GameObject _MainMenu;               //Main Menu
        public GameObject _MultiplayerMenu;        //All Menus that are networking relevant
        public GameObject _CreateRoomMenu;         //Menu for room creation
        public GameObject _RoomMenu;               //Like a actual game lobby
        public GameObject _RoomSetupMenu;          //"Host" only menu for changing settings like maxplayers or open/close state
        public GameObject _LobbyMenu;              //Menu that shows running rooms

        //ButtonsForFunctions
        [Header("Buttons")]
        public GameObject _ButtonMainNewGame;      //Open game room creation menu
        public GameObject _ButtonCreateRandomRoom; //Creates a room with random name and default settings
        public GameObject _ButtonCreateCustomRoom; //Shows room menu and opens room setup menu
        public GameObject _ButtonCreateToMain;     //Button Back to main Menu
        public GameObject _ButtonCancelSetup;      //Cancels custom room setup
        public GameObject _ButtonJoinLobby;        //Open up lobby menu to see running rooms
        public GameObject _ButtonLobbyNewGame;     //Create a new game from lobby
        public GameObject _ButtonLobbyToMain;      //Leave lobby to main menu
        public GameObject _ButtonStatsMenu;        //Open statistics menu
        public GameObject _ButtonStatsToMain;      //Leave statistics to main menu
        public GameObject _ButtonExitApp;          //Close game

        //InputFields for data to check
        [Header("Input Fields")]
        public TextMeshProUGUI _InputPlayerName;
        public TextMeshProUGUI _InputRoomName;
        public TextMeshProUGUI _InputStartTimer;
        public TextMeshProUGUI _InputMaxPlayers;
        public TextMeshProUGUI _InputMaxKills;
        public TextMeshProUGUI _Input

    //TextFields for data to change
    [Header("Text Fields"))]



        #endregion

        #region Methods

        private void Awake()
        {
            UIManagerSingleton();
        }

        protected void UIManagerSingleton()
        {
            if (UI_Manager.Manager == null)
            {
                UI_Manager.Manager = this;
            }
            else
            {
                if (UI_Manager.Manager != this)
                {
                    Destroy(UI_Manager.Manager.gameObject);
                    UI_Manager.Manager = this;
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }

        #endregion
    }
}
