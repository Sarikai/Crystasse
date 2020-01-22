using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UnityEngine.CreateAssetMenu(), System.Serializable]
public class UnitData : ScriptableObject
{
    [Header("Behaviour")]
    public float Range;

    [Range(0, 255)]
    public byte HealthPoints, AttackPoints, BuildPoints;
    public float BuildSpeed, MoveSpeed;

}
