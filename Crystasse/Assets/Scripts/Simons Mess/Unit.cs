using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Agent
{
    private static int _lastID = 0;
    [SerializeField]
    private UnitData _data;

    public int ID { get; private set; }
    public byte TeamID => _data.TeamID;
    public byte Health { get; private set; }
    public byte AttackPoints => _data.AttackPoints;
    public byte BuildPoints { get; private set; }

    public byte BuildingPoints
    {
        get
        {
            var value = (byte)_data.BuildSpeed;
            BuildPoints -= value;

            return value;
        }
    }

    private void Awake()
    {
        ID = _lastID;
        _lastID++;

        Health = _data.HealthPoints;
        BuildPoints = _data.BuildPoints;
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
}
