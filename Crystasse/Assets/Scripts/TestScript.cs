using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;

public class TestScript : MonoBehaviour
{
    EntityArchetype _unitArchetype;
    [SerializeField]
    UnitData _data = null;
    World _world = null;
    [SerializeField]
    int _maxUnits = 5, _batchSize = 5;

    [SerializeField]
    float _time = 5f;
    float _timer = 5f;

    private void Start()
    {
        _world = World.Active;
        _unitArchetype = UnitData.Archetype;

        var g = new InstancedRenderMeshBatchGroup(
            _world.EntityManager,
            _world.GetExistingSystem<RenderMeshSystemV2>(),
            _world.GetExistingSystem<RenderMeshSystemV2>().EntityQueries[1]);
    }
}
