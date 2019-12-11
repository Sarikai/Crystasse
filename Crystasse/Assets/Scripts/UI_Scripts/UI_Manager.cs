using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PUN_Network;
using System;
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

        //MenusToToggle
        [Header("Menus")]
        public GameObject _MainMenu;               //Main Menu
        public GameObject _MultiplayerMenu;        //All Menus that are networking relevant
        public GameObject _RoomCreateMenu;         //Menu for room creation
        public GameObject _RoomMenu;               //Like a actual game lobby
        public GameObject _RoomSetupMenu;          //"Host" only menu for changing settings like maxplayers or open/close state
        public GameObject _LobbyMenu;              //Menu that shows running rooms
        public GameObject _IngameMenu;             //Main menu in active game
        public GameObject _StatsMenu;              //Statistics menu
        public GameObject _HUD;

        //ButtonsForFunctions
        [Header("Buttons")]
        public GameObject _ButtonMainNewGame;      //Open game room creation menu
        public GameObject _ButtonCreateRandomRoom; //Creates a room with random name and default settings
        public GameObject _ButtonCreateCustomRoom; //Shows room menu and opens room setup menu
        public GameObject _ButtonCreateToMain;     //Button Back to main Menu
        public GameObject _ButtonCancelSetup01;    //Cancels custom room setup
        public GameObject _ButtonCancelSetup02;    //Cancels room settings setup
        public GameObject _ButtonApplySetup01;     //Applies custom room setup
        public GameObject _ButtonApplySetup02;     //Applies room settings setup
        public GameObject _ButtonJoinLobby;        //Open up lobby menu to see running rooms
        public GameObject _ButtonLobbyNewGame;     //Create a new game from lobby
        public GameObject _ButtonLobbyToMain;      //Leave lobby to main menu
        public GameObject _ButtonStartGame;
        public GameObject _ButtonRoomToMain;
        public GameObject _ButtonLeaveRoom;
        public GameObject _ButtonRoomSettings;
        public GameObject _ButtonStatsMenu;        //Open statistics menu
        public GameObject _ButtonStatsToMain;      //Leave statistics to main menu
        public GameObject _ButtonExitApp;          //Close game

        //InputFields for data needed
        [Header("Input Fields")]
        public TextMeshProUGUI _InputPlayerName;   //Input for a players nickname
        public TextMeshProUGUI _InputRoomName;     //Input for the name of a custom room
        public TextMeshProUGUI _InputMaxPlayers;   //Input for maximum players in room
        public TextMeshProUGUI _InputMaxUnits;     //Input for maximal units --> create dropdown selector for global, per player, per crystal
        public TextMeshProUGUI _InputCrystalAmount;//Input for crystal amount on map
        //[Range(0, 20)]
        public TextMeshProUGUI _InputStartTimer;   //Input for delayed start time
        public bool _Autostart;

        //TextFields for data to change
        [Header("Data Fields")]
        public TextMeshProUGUI _RoomName;          //Name of room player has joined
        public Transform _ServerList;
        public Transform _PlayerList;

        //HUD 
        [Header("HUD")]
        public TextMeshProUGUI _PlayerName;        //Name of player
        public TextMeshProUGUI _CrystalsNeutral;   //All crystals that are neutral
        public TextMeshProUGUI _CrystalsOwned;     //All crystals the player owns
        public TextMeshProUGUI _CrystalsEnemy;     //All crystals owned by enemies
        public TextMeshProUGUI _Population;        //Population player actual has
        public TextMeshProUGUI _PopulationLimit;   //Players maximal allowed population
        public TextMeshProUGUI _TimeRunning;       //Shows how long game is up
        public TextMeshProUGUI _RealTime;          //Shows the real time of reality
        public TextMeshProUGUI _SelectedUnitAmount;//Shows units selected with selection

        //GradientColors
        [Header("Gradients")]
        public Gradient _blueGradient;
        public Gradient _greemGradient;
        public Gradient _orangeGradient;
        public Gradient _redGradient;
        public Gradient _blackGradient;

        //Others
        [Header("Others")]
        public GameObject _Background;
        public PUN_ServerlistEntry _serverEntryPrefab;
        public PUN_PlayerlistEntry _playerEntryPrefab;

        #endregion

        #region Methods

        //All toggle functions
        #region Toggles

        public void Toggle(GameObject objectToToggle)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }

        public void ToggleMainMenu()
        {
            Toggle(_MainMenu);
            //RoomOptions roomops = new RoomOptions();
            //Room myroom = new Room("", roomops);
            //myroom.SetCustomProperties(GameManager.MasterManager.NetworkManager.SetRoomSettings());

        }

        public void ToggleMultiplayerMenu()
        {
            Toggle(_MultiplayerMenu);
        }

        public void ToggleCreateRoomMenu()
        {
            Toggle(_RoomCreateMenu);
        }

        public void ToggleRoomMenu()
        {
            Toggle(_RoomMenu);
        }

        public void ToggleRoomSetupMenu()
        {
            Toggle(_RoomSetupMenu);
        }

        public void ToggleRoomSetupButtons()
        {
            Toggle(_ButtonApplySetup01);
            Toggle(_ButtonApplySetup02);
            Toggle(_ButtonCancelSetup01);
            Toggle(_ButtonCancelSetup02);
        }

        public void ToggleLobbyMenu()
        {
            Toggle(_LobbyMenu);
        }

        public void ToggleIngameMenu()
        {
            Toggle(_IngameMenu);
        }

        public void ToggleStatsMenu()
        {
            Toggle(_StatsMenu);
        }

        public void ToggleHUD()
        {
            Toggle(_HUD);
        }

        #endregion

        //All Button functions
        #region Buttons

        public virtual void OnButtonNewGameClicked()
        {
            ToggleMainMenu();
            ToggleMultiplayerMenu();
            ToggleCreateRoomMenu();
        }

        public virtual void OnButtonCreateRandomRoomClicked()
        {
            ToggleCreateRoomMenu();
            ToggleRoomMenu();
            GameManager.MasterManager.NetworkManager.CreateRoom();
        }

        public virtual void OnButtonCreateCustomRoomClicked()
        {
            ToggleCreateRoomMenu();
            ToggleRoomMenu();
            ToggleRoomSetupMenu();
        }

        public virtual void OnButtonCreateToMainClicked()
        {
            ToggleCreateRoomMenu();
            ToggleMultiplayerMenu();
            ToggleMainMenu();
        }

        public virtual void OnButtonCancelSetup01Clicked()
        {
            ToggleRoomSetupMenu();
            ToggleRoomMenu();
            ToggleMultiplayerMenu();
            ToggleMainMenu();
        }

        public virtual void OnButtonCancelSetup02Clicked()
        {
            ToggleRoomSetupMenu();
        }

        public virtual void OnButtonApplySetup01Clicked()
        {
            GameManager.MasterManager.NetworkManager.CreateRoom(_InputRoomName.text);
            ToggleRoomSetupButtons();
            ToggleRoomSetupMenu();
        }

        public virtual void OnButtonApplySetup02Clicked()
        {
            GameManager.MasterManager.NetworkManager.UpdateRoomSettings();
            //GameManager.MasterManager.NetworkManager.OnRoomListUpdate(GameManager.MasterManager.NetworkManager.GetLobby.Rooms);
        }

        public virtual void OnButtonJoinLobbyClicked()
        {
            GameManager.MasterManager.NetworkManager.JoinDefaultLobby();
            ToggleMainMenu();
            ToggleMultiplayerMenu();
            ToggleLobbyMenu();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
        }

        public virtual void OnButtonLobbyNewGameClicked()
        {
            ToggleLobbyMenu();
            ToggleCreateRoomMenu();
        }

        public virtual void OnButtonLobbyToMainClicked()
        {
            ToggleLobbyMenu();
            ToggleMultiplayerMenu();
            ToggleMainMenu();
        }

        public virtual void OnButtonJoinRoomClicked(String roomName)
        {
            GameManager.MasterManager.NetworkManager.JoinRoom(roomName);
            ToggleLobbyMenu();
            ToggleRoomMenu();
        }

        public virtual void OnButtonRoomToMainClicked()
        {
            GameManager.MasterManager.NetworkManager.LeaveRoom();
            ToggleRoomMenu();
            ToggleMultiplayerMenu();
            ToggleMainMenu();
        }

        public virtual void OnButtonRoomLeaveClicked()
        {
            GameManager.MasterManager.NetworkManager.LeaveRoom();
            GameManager.MasterManager.NetworkManager.JoinDefaultLobby();
            ToggleRoomMenu();
            ToggleLobbyMenu();
        }

        public virtual void OnButtonRoomSettingsClicked()
        {
            ToggleRoomSetupMenu();
        }

        public virtual void OnButtonStartGameClicked()
        {
            GameManager.MasterManager.MainView.RPC("RPC_StartGame", RpcTarget.All);
        }

        public virtual void OnButtonStatsMenuClicked()
        {

        }

        public virtual void OnButtonStatsToMainClicked()
        {

        }

        public virtual void OnButtonExitAppClicked()
        {
            Application.Quit();
            Debug.Log("App Quit");
        }

        public virtual void HUD_Update()
        {

        }

        #endregion

        #endregion
    }
}