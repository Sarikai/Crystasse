using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private readonly Dictionary<int, CrystalConnections> _bridges;

    public CrystalConnections this[int id]
    {
        get
        {
            if(_bridges.ContainsKey(id))
                return _bridges[id];
            else
                return null;
        }
    }
    public bool this[int id, int index]
    {
        get
        {
            if(_bridges.ContainsKey(id))
                return _bridges[id][index];
            else
                return false;
        }
    }

    public Map() { }
    public void AddConnection(int id, CrystalConnections connections)
    {
        if(!_bridges.ContainsKey(id))
            _bridges.Add(id, connections);
    }
}
