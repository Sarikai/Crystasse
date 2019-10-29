using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype
{
    public class PUN_Room : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties 
        public static PUN_Room room;
        private PhotonView pv;

        public bool isGameLoaded;
        public int currentScene;

        Player[] photonPlayers;
        public int playersInRoom;
        public int myNumberInRoom;
        public int playerInGame;

        private bool readyToCount;
        private bool readyToStart;
        public float startingTime;
        private float lessThanMaxPlayers;
        private float atMaxPlayers;
        private float timeToStart;
        #endregion

        #region Methods 
        private void Awake()
        {
            if (PUN_Room.room == null)
            {
                PUN_Room.room = this;

            }
            else
            {
                if (PUN_Room.room != this)
                {
                    Destroy(PUN_Room.room.gameObject);
                    PUN_Room.room = this;
                }

            }
            DontDestroyOnLoad(this.gameObject);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }

        private void Start()
        {
            pv = GetComponent<PhotonView>();
            readyToCount = false;
            readyToStart = false;
            lessThanMaxPlayers = startingTime;
            atMaxPlayers = 6;
            timeToStart = startingTime;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Connected to Room");
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            myNumberInRoom = playersInRoom;
            PhotonNetwork.NickName = myNumberInRoom.ToString();
            if (ServerSetting.multiplayerSetting.delayStart)
            {
                Debug.Log($"Display players in room out of max players possible ({playersInRoom} : {ServerSetting.multiplayerSetting.maxPlayers})");
                if (playersInRoom > 1)
                {
                    readyToCount = true;
                }
                if (playersInRoom == ServerSetting.multiplayerSetting.maxPlayers)
                {
                    readyToStart = true;
                    if (!PhotonNetwork.IsMasterClient)
                        return;
                    PhotonNetwork.CurrentRoom.IsOpen = false;

                }
            }
            else
            {
                StartGame();
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("A new Spast joined");
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom++;
            if (ServerSetting.multiplayerSetting.delayStart)
            {
                Debug.Log($"Display players in room out of max players possible ({playersInRoom} : {ServerSetting.multiplayerSetting.maxPlayers})");
                if (playersInRoom > 1)
                {
                    readyToCount = true;
                }
                if (playersInRoom == ServerSetting.multiplayerSetting.maxPlayers)
                {
                    readyToStart = true;
                    if (!PhotonNetwork.IsMasterClient)
                        return;
                    PhotonNetwork.CurrentRoom.IsOpen = false;

                }
            }
        }



        private void Update()
        {
            if (ServerSetting.multiplayerSetting.delayStart)
            {
                if (playersInRoom == 1)
                {
                    RestartTimer();
                }

                if (!isGameLoaded)
                {
                    if (readyToStart)
                    {
                        atMaxPlayers -= Time.deltaTime;
                        lessThanMaxPlayers = atMaxPlayers;
                        timeToStart = atMaxPlayers;
                    }
                    else if (readyToCount)
                    {
                        lessThanMaxPlayers -= Time.deltaTime;
                        timeToStart = lessThanMaxPlayers;
                    }
                    Debug.Log($"Display time to start: {timeToStart}");
                    if (timeToStart <= 0)
                    {
                        StartGame();
                    }
                }
            }
        }

        void StartGame()
        {
            isGameLoaded = true;
            if (!PhotonNetwork.IsMasterClient)
                return;
            if (ServerSetting.multiplayerSetting.delayStart)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

            }
            PhotonNetwork.LoadLevel(ServerSetting.multiplayerSetting.multiplayerScene);

        }

        void RestartTimer()
        {
            lessThanMaxPlayers = startingTime;
            timeToStart = startingTime;
            atMaxPlayers = 6;
            readyToCount = false;
            readyToStart = false;
        }

        void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            currentScene = scene.buildIndex;
            if (currentScene == ServerSetting.multiplayerSetting.multiplayerScene)
            {
                isGameLoaded = true;
                if (ServerSetting.multiplayerSetting.delayStart)
                {
                    pv.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
                }
                else
                {
                    RPC_CreatePlayer();
                }
            }
        }

        [PunRPC]
        private void RPC_LoadedGameScene()
        {
            playerInGame++;
            if (playerInGame == PhotonNetwork.PlayerList.Length)
            {
                pv.RPC("RPC_CreatePlayer", RpcTarget.All);
            }
        }

        [PunRPC]
        private void RPC_CreatePlayer()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        }
        #endregion
    }
}
