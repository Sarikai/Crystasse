using System.Collections;
using System.Collections.Generic;
using System;

//TODO: Make traversable
public class Tree<T> : IEnumerable<Node<T>>
{
    public Node<T> Root { get; set; }
    public int Length { get; protected set; }
    public bool IsEmpty => Length <= 0;

    public Node<T> this[int index]
    {
        get
        {
            return DepthFirst(index);
        }
        private set
        {
            if(value == null)
                throw new ArgumentNullException();
            Replace(value, index);
        }
    }

    public Tree() { }

    public Tree(Node<T> root)
    {
        Root = root;
        Length++;
    }

    public void Add(Node<T> node)
    {
        if(node == null)
            throw new ArgumentNullException();

        var conns = node.ConnectionsIn;

        foreach(var conn in conns)
            foreach(var current in this)
                if(conn == current)
                    current.ConnectionsOut.Add(node);

        conns = node.ConnectionsOut;

        foreach(var conn in conns)
            foreach(var current in this)
                if(conn == current)
                    current.ConnectionsIn.Add(node);

        Length++;
    }

    public void RemoveAt(int index)
    {
        Remove(DepthFirst(index));
    }

    public void Remove(Node<T> node)
    {
        foreach(var current in this)
        {
            if(current != node && current.ConnectionsIn.Contains(node))
                current.ConnectionsIn.Remove(node);
            if(current != node && current.ConnectionsOut.Contains(node))
                current.ConnectionsOut.Remove(node);
        }
        Length--;
    }

    public void Replace(Node<T> node, int index)
    {
        var toReplace = DepthFirst(index);

        foreach(var current in this)
        {
            if(current != toReplace && current.ConnectionsIn.Contains(toReplace))
            {
                current.ConnectionsIn.Remove(toReplace);
                current.ConnectionsIn.Add(node);
            }
            if(current != toReplace && current.ConnectionsOut.Contains(toReplace))
            {
                current.ConnectionsOut.Remove(toReplace);
                current.ConnectionsOut.Add(node);
            }
        }

        node.ConnectionsIn = toReplace.ConnectionsIn;
        node.ConnectionsOut = toReplace.ConnectionsOut;
    }

    public bool Contains(Node<T> node)
    {
        foreach(var current in this)
            if(current == node)
                return true;
        return false;
    }

    public int IndexOf(Node<T> node)
    {
        int index = 0;
        if(Contains(node))
            foreach(var current in this)
                if(current == node)
                    return index;
                else
                    index++;

        return -1;
    }

    private Node<T> DepthFirst(int index)
    {
        if(index < 0 || index >= Length)
            throw new System.IndexOutOfRangeException();
        else if(index == 0)
            return Root;

        var nodeList = new List<Node<T>>();
        nodeList.AddRange(Root.ConnectionsIn);
        for(int i = 1; i < index; i++)
        {
            if(nodeList.Count > 0)
            {
                var node = nodeList[0];
                nodeList.RemoveAt(0);
                if(i == index)
                    return node;

                if(node.ConnectionsIn.Count > 0)
                    nodeList.InsertRange(0, node.ConnectionsIn);
            }
        }
        return null;
    }

    public IEnumerator<Node<T>> GetEnumerator()
    {
        for(int i = 0; i < Length; i++)
            yield return DepthFirst(i);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
