using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class Unit : Agent
{
    private static int _lastID = 0;
    [SerializeField]
    private UnitData _data;
    [SerializeField]
    private SphereCollider _collider, _rangeTrigger;

    public int ID { get; private set; }
    public byte TeamID => _data.TeamID;
    public byte Health { get; private set; }
    public byte AttackPoints => _data.AttackPoints;
    public byte BuildPoints { get; private set; }
    public float MoveSpeed => _data.MoveSpeed;

    public byte BuildingPoints
    {
        get
        {
            var value = (byte)_data.BuildSpeed;
            BuildPoints -= value;

            return value;
        }
    }

    public float BuildSpeed => _data.BuildSpeed;

    private void Awake()
    {
        ID = _lastID;
        _lastID++;

        Health = _data.HealthPoints;
        BuildPoints = _data.BuildPoints;
        _rangeTrigger.radius = _data.Range;
    }

    private void Start()
    {
        StateMachine.SwitchState(this, new IdleState(transform, _data.MoveSpeed));
    }

    public void UpdateUnit()
    {
        if(!CurrentState.Completed)
            CurrentState.UpdateState();
    }

    public void TakeDamage(byte value)
    {
        if(value >= Health)
            Die();
        else
            Health -= value;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    public void SwitchState(State state)
    {
        CurrentState = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Unit>();

        if(enemy && enemy.TeamID != TeamID)
            StateMachine.SwitchState(this, new AttackState(this, enemy));
    }
}
