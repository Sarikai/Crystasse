using CustomUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using PUN_Network;
using System.Linq;

namespace PUN_Network
{
    [RequireComponent(typeof(PhotonView), typeof(InputManager))]
    public class PUN_CustomPlayer : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties

        [SerializeField]
        private Crystal _crystalPrefab;
        [SerializeField]
        private Player _localPlayer;
        [SerializeField]
        private string _nickName = "New Enligthened";
        [SerializeField]
        private byte _teamID;
        [SerializeField]
        private int _actorNumber;
        [SerializeField]
        private Unit _unitPrefab;
        [SerializeField]
        private Match _matchSession;
        [SerializeField]
        private InputManager _inputManager;


        //Properties
        public int ActorNumber { get => _actorNumber; set => _actorNumber = value; }
        public byte TeamID { get => _teamID; set => _teamID = value; }
        public string NickName { get => _nickName; set => _nickName = value; }
        public Player LocalPlayer { get => _localPlayer; set => _localPlayer = value; }
        public Crystal CrystalPrefab { get => _crystalPrefab; set => _crystalPrefab = value; }
        public Unit UnitPrefab { get => _unitPrefab; set => _unitPrefab = value; }
        public InputManager InputManager { get => _inputManager; set => _inputManager = value; }

        #endregion

        #region Methods


        #region RPCs



        #endregion


        #endregion
    }
}
