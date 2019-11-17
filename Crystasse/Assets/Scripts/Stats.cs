using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CustomUI
{
	public class Stats : MonoBehaviourPunCallbacks
	{
        #region Variables / Properties

        uint spawnedUnits;
        uint destroyedUnits;


		#endregion
		
		#region Methods
		
        public void IncrementSpawns()
        {
            spawnedUnits++;
        }

        public void ResetSpawns()
        {
            spawnedUnits = 0;
        }

        public void IncrementKills()
        {
            destroyedUnits++;
        }

        public void ResetKills()
        {
            destroyedUnits = 0;
        }

		#endregion
	}
}
