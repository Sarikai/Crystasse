using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalConnections
{
    public readonly int From;
    public int[] To = new int[32];
    private int _areBuilt = 0;

    public bool this[int i]
    {
        get
        {
            // Shift bit to examine onto the first bit, then set all other bits to 0.
            // Afterwards check the value of the integer against 0 (false) and 1 (true).
            return ((_areBuilt >> i) & 1) == 1 ? true : false;
        }
        set
        {
            if(value)
                _areBuilt = 1 << i;
            else
                _areBuilt = 0 << i;
        }
    }

    public CrystalConnections(int id)
    {
        From = id;
    }
    public CrystalConnections(int id, int[] to)
    {
        From = id;
        for(int i = 0; i < to.Length && i < 32; i++)
            To[i] = to[i];
    }
}
