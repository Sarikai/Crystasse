using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using Unity.Burst;
using Unity.Entities;

[UnityEngine.CreateAssetMenu(), BurstCompile, System.Serializable]
public class CrystalEntityData : UnityEngine.ScriptableObject
{
    public int ID;
    public byte TeamID;
    public float Range;
}