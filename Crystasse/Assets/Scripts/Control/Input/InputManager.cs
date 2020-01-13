using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Camera _cam;
    [SerializeField]
    float _camSpeed = 5f;

    public Area _playArea { get; private set; }

    public byte _myTeamID;

    //Vector3 _selectionStart;

    public void Init(Area area)
    {
        _playArea = area;
    }


    private void Update()
    {
        //TODO: check again if correct
        if (GameManager.MasterManager.NetworkManager.photonView.IsMine)
        {
            if (_cam == null)
                _cam = FindObjectOfType<Camera>();

            //MakeSelection();

            RaycastHit hit;

            if (Input.GetMouseButtonDown(0) && (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit, 1000, Selection.PlaneLayer)))
                Selection.CastSphereSelection(hit);

            MoveCam(Time.deltaTime);

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
}
