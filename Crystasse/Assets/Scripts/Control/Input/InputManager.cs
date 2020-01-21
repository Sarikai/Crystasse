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
    float _camSpeed = 50f, _camRotSpeed = 60f;

    private Crystal selCrystal;

    public Area _playArea { get; private set; }

    public byte _teamID;

    Vector3 _selectionStart;
    private void Start()
    {
        //TODO: Remove
        Init(new Area(new Vector2(-1000000000, -1000000000), new Vector2(1000000000, 1000000000)), FindObjectOfType<Camera>());
    }

    public void Init(Area area)
    {
        _playArea = area;
    }

    public void Init(Area area, Camera cam)
    {
        _playArea = area;
        _cam = cam;
        if(_cam == null)
            Debug.LogError("No Cam on inputmanager");
    }

    private void Update()
    {
        //TODO: check again if photonView check is correct or still needed
        //if (GameManager.MasterManager.NetworkManager.photonView.IsMine)
        {
            if(_cam == null)
            {
                Debug.LogError("No Cam on inputmanager");
                _cam = FindObjectOfType<Camera>();
                return;
            }

            RaycastHit hit;

            MoveCam(Time.deltaTime * _camSpeed);

            if(Input.GetMouseButtonDown(0))
            {
                if(RayCastToMouse(Selection.CrystalLayer, out hit))
                {
                    var c = hit.collider.GetComponent<Crystal>();
                    if(c != null)
                    {
                        selCrystal = c;
                        var bridges = BridgeList.GetBridges(c);

                        foreach(var b in bridges)
                            b.Show(true);
                    }
                }
                else if(RayCastToMouse(Selection.BridgeLayer, out hit) && Selection.HasValidSelection)
                {
                    var bridge = hit.collider.GetComponent<Bridge>();
                    if(bridge != null)
                        foreach(var unit in Selection.Selected)
                            if(unit != null)
                                StateMachine.SwitchState(unit, new BuildState(unit, bridge));
                }
                else if(RayCastToMouse(Selection.PlaneLayer, out hit))
                {
                    var bridges = BridgeList.GetBridges(selCrystal);

                    foreach(var b in bridges)
                        b.Show(false);

                    selCrystal = null;

                    Selection.CastSphereSelection(hit);

                    StopCoroutine(BoxSelectionRoutine());
                    StartCoroutine(BoxSelectionRoutine());
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
