﻿using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;

public class TestScript : MonoBehaviour
{
    private static TestScript _instance;
    public static TestScript Instance { get => _instance; private set => _instance = value; }

    EntityArchetype _unitArchetype;
    [SerializeField]
    UnitData _data = null;
    World _world = null;
    [SerializeField]
    int _maxUnits = 5, _batchSize = 5;

    [SerializeField]
    float _time = 5f;
    float _timer = 5f;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(this);
    }

    private void Start()
    {
        _world = World.Active;
        _unitArchetype = UnitData.Archetype;
    }

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;

        if(_timer >= _time && _maxUnits > 0)
        {
            for(int i = 0; i < _batchSize; i++)
                AssignDefaultValues(_world.EntityManager.CreateEntity(_unitArchetype));
            _maxUnits -= _batchSize;
            _timer = 0f;
        }
    }

    public Entity CreateEntity(World world)
    {
        var e = world.EntityManager.CreateEntity(_unitArchetype);
        AssignDefaultValues(e);
        return e;
    }

    private void AssignDefaultValues(Entity e)
    {
        _world.EntityManager.SetSharedComponentData<TeamID>(e, _data.teamID);
        _world.EntityManager.SetSharedComponentData<RenderMesh>(e, _data.m);

        _world.EntityManager.SetComponentData<ID>(e, _data.id);
        _world.EntityManager.SetComponentData<Translation>(e, _data.t);
        _world.EntityManager.SetComponentData<Scale>(e, _data.s);
        _world.EntityManager.SetComponentData<AttackPoints>(e, _data.ap);
        _world.EntityManager.SetComponentData<BuildPoints>(e, _data.bp);
        _world.EntityManager.SetComponentData<HealthPoints>(e, _data.hp);
        _world.EntityManager.SetComponentData<BuildSpeed>(e, _data.bs);
        _world.EntityManager.SetComponentData<MoveSpeed>(e, _data.ms);
        _world.EntityManager.SetComponentData<State>(e, _data.sD);
        _world.EntityManager.SetComponentData<Target>(e, _data.target);
        _world.EntityManager.SetComponentData<IdleData>(e, UnitData.DefaultIdleData);
    }
}
