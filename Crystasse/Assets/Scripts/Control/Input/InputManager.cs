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
    float _camSpeed = 50f, _camRotSpeed = 60f, _scrollSpeed = 100f;

    private List<Crystal> _selCrystals = new List<Crystal>();

    public Area _playArea { get; private set; }

    public byte _teamID;
    private List<Bridge> _shownBridges = new List<Bridge>();

    Vector3 _selectionStart;

    public void Init(Area area)
    {
        _playArea = area;
    }

    public void Init(Area area, Camera cam)
    {
        _playArea = area;
        _cam = cam;
    }

    private void Update()
    {
        if(_cam == null)
            return;

        RaycastHit hit;

        MoveCam(Time.deltaTime * _camSpeed);

        if(Input.GetKeyDown(KeyCode.F))
        {
            List<Crystal> crystals = new List<Crystal>(GameManager.MasterManager.NetworkManager.MapData.Crystals);
            crystals.AddRange(GameManager.MasterManager.NetworkManager.MapData.Bases);

            List<Bridge> bridges = new List<Bridge>();
            foreach(var c in crystals)
                if(c.IsMyTeam)
                    bridges.AddRange(BridgeList.GetBridges(c));

            foreach(var b in bridges)
                b.Show(true);
            _shownBridges = bridges;
        }

        if(Input.GetKeyUp(KeyCode.F))
        {
            foreach(var b in _shownBridges)
                b.Show(false);
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(RayCastToMouse(Selection.BridgeLayer, out hit) && Selection.HasValidSelection)
            {
                var bridge = hit.collider.GetComponentInParent<Bridge>();
                if(bridge != null)
                    foreach(var unit in Selection.Selected)
                        if(unit != null)
                            StateMachine.SwitchState(unit, new BuildState(unit, bridge));
            }
            else if(RayCastToMouse(Selection.PlaneLayer, out hit))
            {
                List<Bridge> bridges = new List<Bridge>();
                foreach(var crystal in _selCrystals)
                    bridges = BridgeList.GetBridges(crystal);

                foreach(var b in bridges)
                    b.Show(false);

                _selCrystals.Clear();

                Selection.CastSphereSelection(hit);
            }
        }

        if(Input.GetMouseButtonDown(1) && Selection.HasValidSelection
       && RayCastToMouse(Selection.PlaneLayer, out hit))
        {
            foreach(var unit in Selection.Selected)
                if(unit != null && !unit.MeshAgent.Raycast(hit.point, out _))
                    StateMachine.SwitchState(unit, new MoveState(unit.MoveSpeed, unit, hit.point, unit.MeshAgent));
        }
    }

    private bool RayCastToMouse(int layerMask, out RaycastHit hit) => Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit, 1000, layerMask);

    private void MoveCam(float speed)
    {
        var camTrans = _cam.transform;

        if(Input.GetKey(KeyCode.A))
            camTrans.position -= camTrans.right * speed;
        if(Input.GetKey(KeyCode.D))
            camTrans.position += camTrans.right * speed;
        if(Input.GetKey(KeyCode.W))
            camTrans.position += new Vector3(camTrans.forward.x, 0, camTrans.forward.z) * speed;
        if(Input.GetKey(KeyCode.S))
            camTrans.position -= new Vector3(camTrans.forward.x, 0, camTrans.forward.z) * speed;
        if(Input.GetKey(KeyCode.Q))
            camTrans.eulerAngles -= Vector3.up * Time.deltaTime * _camRotSpeed;
        else if(Input.GetKey(KeyCode.E))
            camTrans.eulerAngles += Vector3.up * Time.deltaTime * _camRotSpeed;
        if(Input.mouseScrollDelta.y > 0.1f && camTrans.position.y > 10f)
            camTrans.position += camTrans.forward * Time.deltaTime * _scrollSpeed;
        if(Input.mouseScrollDelta.y < -0.1f && camTrans.position.y < 80f)
            camTrans.position -= camTrans.forward * Time.deltaTime * _scrollSpeed;
    }

    private IEnumerator BoxSelectionRoutine()
    {
        _selectionStart = Input.mousePosition;

        while(!Input.GetMouseButtonUp(0))
            yield return new WaitForEndOfFrame();

        Selection.CastBoxSelection(_selectionStart, Input.mousePosition);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
            stream.SendNext(_teamID);
        else
            _teamID = (byte)stream.ReceiveNext();
    }
}
