using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;

[UnityEngine.CreateAssetMenu(), BurstCompile, System.Serializable]
public class UnitData : ScriptableObject
{
    [Header("Identification")]
    public int ID;
    [Range(0, 255)]
    public byte TeamID;

    [Header("Behaviour")]
    public float Range;

    [Range(0, 255)]
    public byte HealthPoints, AttackPoints, BuildPoints;
    public float BuildSpeed, MoveSpeed;

}
