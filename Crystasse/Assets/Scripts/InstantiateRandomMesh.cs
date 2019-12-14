using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Crystal))]
public class InstantiateRandomMesh : MonoBehaviour
{
    [SerializeField]
    Crystal _crystal;
    [SerializeField]
    Material[] _teamMaterials = null;
    [SerializeField]
    GameObject[] _meshes = null;

    GameObject _gameObject;

    private void Awake()
    {
        InstantiateMesh();
    }

    public void InstantiateMesh()
    {
        var index = Random.Range(0, _meshes.Length);
        if(_gameObject)
            Destroy(_gameObject);
        _gameObject = GameObject.Instantiate(_meshes[index], transform);
        GetComponentInChildren<Transform>().localPosition = Vector3.zero;
        _gameObject.transform.localPosition = Vector3.zero;
    }

    public void ChangeMaterial(PhotonView crystalView)
    {
        crystalView.RPC("RPC_ChangeMaterial", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPC_ChangeMaterial()
    {
        var meshRenderer = _gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.material = _teamMaterials[_crystal.TeamID];
    }

    private void OnValidate()
    {
        _crystal = GetComponent<Crystal>();
    }
}
