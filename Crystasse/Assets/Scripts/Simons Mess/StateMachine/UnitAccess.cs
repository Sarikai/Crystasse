using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UnitAccess
{
    public byte Health { get; private set; }
    public byte AttackPoints { get; private set; }
    public byte BuildPoints { get; private set; }
    public byte BuildingPoints
    {
        get
        {
            var value = (byte)BuildSpeed;
            BuildPoints -= value;

            return value;
        }
    }
    public float BuildSpeed { get; private set; }
    public byte TeamID { get; private set; }

    public UnitAccess(Unit unit)
    {
        Health = unit.Health;
        AttackPoints = unit.AttackPoints;
        BuildPoints = unit.BuildPoints;
        BuildSpeed = unit.BuildSpeed;
        TeamID = unit.TeamID;
    }

    public void TakeDamage(byte value)
    {
        if(value >= Health)
            Health = 0;
        else
            Health -= value;
    }
}
