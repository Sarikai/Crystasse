using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStarter : MonoBehaviour
{
    [SerializeField]
    GameObject _masterManagerPrefab;
    private void Awake()
    {
        if (GameManager.MasterManager == null)
            Instantiate(_masterManagerPrefab);
    }
}
