using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Camera _cam;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, Selection.PlaneLayer))
            {
                Selection.CastSphereSelection(hit);
            }
        }
        if(Input.GetMouseButtonDown(1) && Selection.Selected != null && Selection.Selected.Length > 0)
        {
            if(Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, Selection.PlaneLayer))
                foreach(var unit in Selection.Selected)
                {
                    StateMachine.SwitchState(unit, new MoveState(unit.MoveSpeed, unit.transform, hit.point));
                }
        }
    }
}
