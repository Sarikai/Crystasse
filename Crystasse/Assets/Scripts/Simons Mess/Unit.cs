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
    private byte id;
    [SerializeField]
    private SphereCollider /*_collider,*/ _attackTrigger;
    [SerializeField]
    public Photon.Pun.PhotonView _view;

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
        id = _data.TeamID;
        ID = _lastID;
        _lastID++;

        _view = GetComponent<Photon.Pun.PhotonView>();
        _view.ViewID = 1100 + ID;
        Health = _data.HealthPoints;
        BuildPoints = _data.BuildPoints;
        _attackTrigger.radius = _data.Range;
    }

    private void Start()
    {
        StateMachine.SwitchState(this, new IdleState(this, _data.MoveSpeed));
    }

    public void UpdateUnit()
    {
        if (!CurrentState.Completed)
            CurrentState.UpdateState();
    }

    public void TakeDamage(byte value)
    {
        if (value >= Health)
            Die();
        else
            Health -= value;
    }

    [Photon.Pun.PunRPC]
    private void Die()
    {
        if (this != null)
            DestroyImmediate(gameObject);
    }
    public void SwitchState(State state)
    {
        CurrentState = state;
    }
}
