using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public static class JobCreator
{
    public static void CreateUpdateUnitJob(Unit[] units)
    {
        //return new UpdateUnitJob() { Units = units }.Schedule(units.Length, units.Length);
    }
}
