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

    private Crystal selCrystal;

    public Area _playArea { get; private set; }

    public byte _teamID;

    Vector3 _selectionStart;
    int crystalLayer = 31;
    int bridgeLayer = 31;
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

            //MakeSelection();

            RaycastHit hit;

            MoveCam(Time.deltaTime);

            if(Input.GetMouseButtonDown(0))
            {
                if(RayCastToMouse(crystalLayer, out hit))
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
                else if(RayCastToMouse(bridgeLayer, out hit) && Selection.HasValidSelection)
                {
                    var bridge = hit.collider.GetComponent<Bridge>();
                    if(bridge != null)
                        foreach(var unit in Selection.Selected)
                        {
                            if(unit != null)
                                StateMachine.SwitchState(unit, new BuildState(unit, bridge));
                        }
                }
                else if(RayCastToMouse(Selection.PlaneLayer, out hit))
                {
                    var bridges = BridgeList.GetBridges(selCrystal);

                    foreach(var b in bridges)
                        b.Show(false);

                    selCrystal = null;

                    Selection.CastSphereSelection(hit);

                    StartCoroutine(BoxSelectionRoutine());
                }
            }


            if(Input.GetMouseButtonDown(1) && Selection.HasValidSelection
           && RayCastToMouse(Selection.PlaneLayer, out hit))
            {
                foreach(var unit in Selection.Selected)
                {
                    if(unit != null)
                        StateMachine.SwitchState(unit, new MoveState(unit.MoveSpeed, unit, hit.point, unit.MeshAgent));
                }
            }

            if(Input.GetKeyDown(KeyCode.T) && Selection.Selected != null && Selection.Selected.Length > 0)
            {
                int inter = UnityEngine.Random.Range(0, Selection.Selected.Length);
                Unit[] selectedUnits = Selection.Selected;

                PhotonNetwork.Destroy(selectedUnits[inter].gameObject);
            }
        }
    }

    private bool RayCastToMouse(int layerMask, out RaycastHit hit) => Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit, 1000, layerMask);

    private void MoveCam(float dt)
    {
        if(Input.GetKey(KeyCode.A))
            _cam.transform.position += Vector3.left * _camSpeed * dt;
        if(Input.GetKey(KeyCode.D))
            _cam.transform.position += Vector3.right * _camSpeed * dt;
        if(Input.GetKey(KeyCode.W))
            _cam.transform.position += Vector3.forward * _camSpeed * dt;
        if(Input.GetKey(KeyCode.S))
            _cam.transform.position += Vector3.back * _camSpeed * dt;
    }

    private void MakeSelection()
    {
        if(Input.GetMouseButtonDown(0) && (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, Selection.PlaneLayer)))
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
        {
            stream.SendNext(_teamID);
        }
        else
        {
            _teamID = (byte)stream.ReceiveNext();
        }
    }
}
