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
    UnitData _prefab;
    World _activeWorld;
    NativeArray<Entity> _entities;
    [SerializeField]
    int _length = 5, _maxUnits = 5;

    [SerializeField]
    float _time = 5f;
    float _timer = 5f;


    private void Start()
    {
        _activeWorld = World.Active;
        _unitArchetype = _activeWorld.EntityManager.CreateArchetype(
                                                   typeof(ID), typeof(TeamID),
                                                   typeof(Translation), typeof(Scale), typeof(LocalToWorld),
                                                   typeof(AttackPoints), typeof(BuildPoints), typeof(HealthPoints),
                                                   typeof(BuildSpeed), typeof(MoveSpeed),
                                                   typeof(AttackRange), typeof(BuildRange), typeof(ConquerRange),
                                                   typeof(State), typeof(Substate), typeof(Target), typeof(RenderMesh), typeof(IdleData));

        _entities = new NativeArray<Entity>(_length, Allocator.Persistent);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= _time && _maxUnits > 0)
        {
            _maxUnits--;
            _timer = 0f;
            var e = _activeWorld.EntityManager.CreateEntity(_unitArchetype);
            _entities[_entities.Length - 1] = e;
            AssignDefaultValues(e);
        }
    }

    private void AssignDefaultValues(Entity e)
    {
        _activeWorld.EntityManager.SetSharedComponentData<TeamID>(e, _prefab.teamID);
        _activeWorld.EntityManager.SetSharedComponentData<RenderMesh>(e, _prefab.m);

        _activeWorld.EntityManager.SetComponentData<ID>(e, _prefab.id);
        _activeWorld.EntityManager.SetComponentData<Translation>(e, _prefab.t);
        _activeWorld.EntityManager.SetComponentData<Scale>(e, _prefab.s);
        _activeWorld.EntityManager.SetComponentData<AttackPoints>(e, _prefab.ap);
        _activeWorld.EntityManager.SetComponentData<BuildPoints>(e, _prefab.bp);
        _activeWorld.EntityManager.SetComponentData<HealthPoints>(e, _prefab.hp);
        _activeWorld.EntityManager.SetComponentData<BuildSpeed>(e, _prefab.bs);
        _activeWorld.EntityManager.SetComponentData<MoveSpeed>(e, _prefab.ms);
        _activeWorld.EntityManager.SetComponentData<State>(e, _prefab.sD);
        _activeWorld.EntityManager.SetComponentData<Target>(e, _prefab.target);
        _activeWorld.EntityManager.SetComponentData<IdleData>(e, new IdleData() { YDirection = -1f });
    }

    private void OnDisable()
    {
        _entities.Dispose();
    }
}
