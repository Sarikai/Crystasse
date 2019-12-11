using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crystal))]
public class InstantiateRandomMesh : MonoBehaviour
{
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
        var meshRenderer = _gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.material = _teamMaterials[_crystal.TeamID];
        meshRenderer.transform.localPosition = Vector3.zero;
        _gameObject.transform.localPosition = Vector3.zero;
    }

    private void OnValidate()
    {
        _crystal = GetComponent<Crystal>();
    }
}
