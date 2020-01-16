using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

[RequireComponent(typeof(Rigidbody), typeof(Photon.Pun.PhotonView))]
public class Unit : Agent
{
    private static int id = 0;
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

    public byte TeamID => _teamID;
    public byte Health { get; private set; }
    public byte AttackPoints => _data.AttackPoints;
    public byte BuildPoints { get; private set; }
    public float MoveSpeed => _data.MoveSpeed;

    public Rigidbody Rigidbody => _rb;

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
        //_view = GetComponent<Photon.Pun.PhotonView>();
        //_view.ViewID = 1100 + id;

        id++;
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
        //TODO: rework to photon.destroy
        if (this != null)
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

        if (_timer >= 2f)
            _timer = 0f;

        var v = new Vector3(0, Constants.MAX_UNIT_DISPLACEMENT * _anims.MoveAnim.Evaluate(_timer), 0) * Time.deltaTime;

        if (_timer <= 1f)
            _visualTrans.position += v;
        else if (_timer > 1f)
            _visualTrans.position -= v;


        //TODO: Rework this pls Simon der Timer wieder global wusste nicht mehr wo du das hingecoded hast hab mir den fix zum testen drüber gecoded
        //if (x <= 1f)
        //    _visualTrans.position += v;
        //else if(x > 1f)
        //    _visualTrans.position -= v;
    }
}
