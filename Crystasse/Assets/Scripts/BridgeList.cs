using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BridgeList
{
    private static List<Bridge> _list = new List<Bridge>();

    public static List<Bridge> GetBridges(Crystal c)
    {
        var l = new List<Bridge>();

        foreach(var b in _list)
            if(b.ConnectsTo(c))
                l.Add(b);

        return l;
    }

    public static void Add(Bridge b)
    {
        _list.Add(b);
    }

    public static void Remove(Bridge b)
    {
        _list.Remove(b);
    }
}
