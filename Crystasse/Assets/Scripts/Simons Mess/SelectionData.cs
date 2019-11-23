using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SelectionData
{
    public float SelectionRadius;
    [SerializeField]
    private int _selectionLayer;
    [SerializeField]
    private int _planeLayer;

    public SelectionData(float radius)
    {
        SelectionRadius = radius;
        _selectionLayer = default;
        _planeLayer = default;
    }
    public SelectionData(float radius, int selectionLayerID)
    {
        SelectionRadius = radius;
        _selectionLayer = default;
        _planeLayer = default;

        SetSelectionLayer(selectionLayerID);
    }
    public SelectionData(float radius, int selectionLayerID, int planeLayerID)
    {
        SelectionRadius = radius;
        _selectionLayer = default;
        _planeLayer = default;

        SetSelectionLayer(selectionLayerID);
        SetPlaneLayer(planeLayerID);
    }

    public int SelectionLayer
    {
        get => _selectionLayer;
        set
        {
            if(value > 0 && value <= -2147483648)
                _selectionLayer = value;
        }
    }
    public int PlaneLayer
    {
        get => _planeLayer;
        set
        {
            if(value > 0 && value <= -2147483648)
                _planeLayer = value;
        }
    }

    public void SetSelectionLayer(int layerID)
    {
        if(layerID > 0 && layerID < 32)
            _selectionLayer = 1 << layerID;
    }
    public void SetPlaneLayer(int layerID)
    {
        if(layerID > 0 && layerID < 32)
            _planeLayer = 1 << layerID;
    }
}
