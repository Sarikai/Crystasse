﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Jobs;

[RequireComponent(typeof(Rigidbody), typeof(Photon.Pun.PhotonView))]
public class Unit : Agent
{
    private static int id = 0;
    [SerializeField]
    private NavMeshAgent _meshAgent = null;
    [SerializeField]
    private UnitData _data;
    [SerializeField]
    private byte _teamID = 0;
    [SerializeField]
    private Rigidbody _rb = null;
    [SerializeField]
    private SphereCollider /*_collider,*/ _attackTrigger = null;
    //[SerializeField]
    //public Photon.Pun.PhotonView _view;
    [SerializeField]
    private Transform _visualTrans = null;
    [SerializeField]
    private UnitAnims _anims = null;
    private float _timer;
    PhotonView _unitView;

    public byte TeamID => _teamID;
    public byte Health { get; private set; }
    public byte AttackPoints => _data.AttackPoints;
    public byte BuildPoints { get; private set; }
    public float MoveSpeed => _data.MoveSpeed;
    public NavMeshAgent MeshAgent => _meshAgent;

    public Rigidbody Rigidbody => _rb;

    public PhotonView UnitView { get => _unitView; set => _unitView = value; }

    public bool IsMyUnit
    {
        get
        {
            Debug.Log($"IsMyUnit: {UnitView.CreatorActorNr == PhotonNetwork.LocalPlayer.ActorNumber}");
            return (UnitView.OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber) /*|| (PhotonNetwork.IsMasterClient && !this.IsOwnerActive)*/;
        }
    }

    public byte BuildingPoints
    {
        get
        {
            byte value;
            if(BuildPoints >= _data.BuildSpeed)
            {
                value = (byte)_data.BuildSpeed;
                BuildPoints -= value;
            }
            else
            {
                value = BuildPoints;
                BuildPoints = 0;
            }

            return value;
        }
    }

    public float BuildSpeed => _data.BuildSpeed;

    private void Awake()
    {
        //_view = GetComponent<Photon.Pun.PhotonView>();
        //_view.ViewID = 1100 + id;

        id++;
        Health = _data.HealthPoints;
        BuildPoints = _data.BuildPoints;
        _attackTrigger.radius = _data.Range;
        UnitView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        StateMachine.SwitchState(this, new IdleState(this, _data.MoveSpeed));
    }

    public void UpdateUnit()
    {
        if(!CurrentState.Completed)
            CurrentState.UpdateState();
    }

    public void TakeDamage(byte value)
    {
        Debug.Log($"Yes I {gameObject} take damage");
        //TODO: Check if check needed, could reduce problem if not
        if (value >= Health /*&& IsMyUnit*/)
            Die();
        else
            Health -= value;
    }


    private void Die()
    {
        //TODO: [DONE] rework to photon.destroy
        if(this != null)
            PhotonNetwork.Destroy(gameObject);
        //DestroyImmediate(gameObject);

    }
    public void SwitchState(State state)
    {
        CurrentState = state;
    }

    public void PlayAttackAnim(Vector3 direction, float x)
    {
        _rb.transform.position += direction * _anims.AttackAnim.Evaluate(x) * Time.deltaTime;
    }

    public void PlayMoveAnim(float x)
    {
        _timer += Time.deltaTime;

        if(_timer >= 2f)
            _timer = 0f;

        var v = new Vector3(0, Constants.MAX_UNIT_DISPLACEMENT * _anims.MoveAnim.Evaluate(_timer), 0) * Time.deltaTime;

        if(_timer <= 1f)
            _visualTrans.position += v;
        else if(_timer > 1f)
            _visualTrans.position -= v;


        //TODO: Rework this pls Simon der Timer wieder global wusste nicht mehr wo du das hingecoded hast hab mir den fix zum testen drüber gecoded
        //if (x <= 1f)
        //    _visualTrans.position += v;
        //else if(x > 1f)
        //    _visualTrans.position -= v;
    }
}
