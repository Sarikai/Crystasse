using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class InputManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    Camera _cam;
    [SerializeField]
    float _camSpeed = 5f;

    public Area _playArea { get; private set; }

    public byte _teamID;

    //Vector3 _selectionStart;

    private void Start()
    {
        Init(new Area(new Vector2(-1000000000, -1000000000), new Vector2(1000000000, 1000000000)));
    }

    public void Init(Area area)
    {
        _playArea = area;
    }


    private void Update()
    {
        //TODO: check again if photonView check is correct or still needed
        //if (GameManager.MasterManager.NetworkManager.photonView.IsMine)
        {
            //TODO: Move to init
            if (_cam == null)
                _cam = FindObjectOfType<Camera>();

            //MakeSelection();

            RaycastHit hit;

            MoveCam(Time.deltaTime);

            if (Input.GetMouseButtonDown(0) && (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit, 1000, Selection.PlaneLayer)))
                Selection.CastSphereSelection(hit);

            if (Input.GetMouseButtonDown(1) && Selection.Selected != null && Selection.Selected.Length > 0
           && (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit, 1000, Selection.PlaneLayer)))
            {
                foreach (var unit in Selection.Selected)
                {
                    if (unit != null)
                        StateMachine.SwitchState(unit, new MoveState(unit.MoveSpeed, unit, hit.point));
                }
            }
        }
    }

    private void MoveCam(float dt)
    {
        if (Input.GetKey(KeyCode.A))
            _cam.transform.position += Vector3.left * _camSpeed * dt;
        if (Input.GetKey(KeyCode.D))
            _cam.transform.position += Vector3.right * _camSpeed * dt;
        if (Input.GetKey(KeyCode.W))
            _cam.transform.position += Vector3.forward * _camSpeed * dt;
        if (Input.GetKey(KeyCode.S))
            _cam.transform.position += Vector3.back * _camSpeed * dt;
    }

    private void MakeSelection()
    {
        if (Input.GetMouseButtonDown(0) && (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, Selection.PlaneLayer)))
            Selection.CastSphereSelection(hit);

        //_selectionStart = hit.point;
        //else if(Input.GetMouseButtonUp(0))
        //{
        //    if(Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, Selection.PlaneLayer))
        //    {
        //        //Selection.CastBoxSelection(_selectionStart, hit.point);
        //    }
        //}
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_teamID);
        }
        else
        {
            _teamID = (byte)stream.ReceiveNext();
        }
    }
}
