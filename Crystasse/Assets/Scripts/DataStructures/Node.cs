using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    public T Value { get; set; }
    public List<Node<T>> ConnectionsIn { get; set; }
    public List<Node<T>> ConnectionsOut { get; set; }
    public bool[] ConnectionActiveIn { get; set; }
    public bool[] ConnectionActiveOut { get; set; }

    public Node(T value)
    {
        Value = value;
    }

    public void Set(T value,
                    List<Node<T>> connectionsIn,
                    List<Node<T>> connectionsOut,
                    bool[] activeIn = null,
                    bool[] activeOut = null)
    {
        Value = value;
        ConnectionsIn = connectionsIn;
        ConnectionsOut = connectionsOut;

        if(activeIn == null)
            ConnectionActiveIn = new bool[ConnectionsIn.Count];
        if(activeOut == null)
            ConnectionActiveOut = new bool[ConnectionsOut.Count];

        SynchronizeLengths();

        if(activeIn != null && activeIn.Length > ConnectionsIn.Count)
        {
            var temp = new bool[ConnectionsIn.Count];
            for(int i = 0; i < temp.Length; i++)
                temp[i] = activeIn[i];
            activeIn = temp;
        }
        if(activeOut != null && activeOut.Length > ConnectionsOut.Count)
        {
            var temp = new bool[ConnectionsOut.Count];
            for(int i = 0; i < temp.Length; i++)
                temp[i] = activeOut[i];
            activeOut = temp;
        }

        if(activeIn != null && activeIn.Length <= ConnectionsIn.Count)
            activeIn.CopyTo(ConnectionActiveIn, 0);
        if(activeOut != null && activeOut.Length <= ConnectionsOut.Count)
            activeOut.CopyTo(ConnectionActiveOut, 0);
    }

    private void SynchronizeLengths()
    {
        if(ConnectionActiveIn.Length < ConnectionsIn.Count)
        {
            var t = new bool[ConnectionsIn.Count];
            ConnectionActiveIn.CopyTo(t, 0);
            ConnectionActiveIn = t;
        }
        if(ConnectionActiveOut.Length < ConnectionsOut.Count)
        {
            var t = new bool[ConnectionsOut.Count];
            ConnectionActiveOut.CopyTo(t, 0);
            ConnectionActiveOut = t;
        }
    }

    ~Node()
    {
        if(Value is IDisposable)
            (Value as IDisposable).Dispose();
    }
}
