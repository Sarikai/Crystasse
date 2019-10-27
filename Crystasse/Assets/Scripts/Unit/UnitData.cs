using System.Collections;
using System.Collections.Generic;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[CreateAssetMenu()]
public class UnitData : ScriptableObject
{
    public ID id;

    public TeamID teamID;

    public Translation t;

    public Scale s;

    public AttackPoints ap;

    public BuildPoints bp;

    public HealthPoints hp;

    public BuildSpeed bs;

    public MoveSpeed ms;

    public State sD;

    public Target target;

    public RenderMesh m;
}
