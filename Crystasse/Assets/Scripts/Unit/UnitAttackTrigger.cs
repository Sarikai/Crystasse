using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackTrigger : MonoBehaviour
{
    [SerializeField]
    private Unit _owner = null;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Collision detected {other}");
        var enemy = other.GetComponent<Unit>();

        if (enemy && enemy.TeamID != _owner.TeamID)
            StateMachine.SwitchState(_owner, new AttackState(_owner, enemy));
    }
}
