using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SelectionData
{
    public float _selectionRadius;
    [SerializeField]
    private int _SelectionLayer;
    [SerializeField]
    private int _PlaneLayer;

    public SelectionData(float radius)
    {
        _selectionRadius = radius;
        _SelectionLayer = default;
        _PlaneLayer = default;
    }
    public SelectionData(float radius, int selectionLayerID)
    {
        _selectionRadius = radius;
        _SelectionLayer = default;
        _PlaneLayer = default;

        SetSelectionLayer(selectionLayerID);
    }
    [Newtonsoft.Json.JsonConstructor]
    public SelectionData(float SelectionRadius, int _selectionLayer, int _planeLayer)
    {
        this._selectionRadius = SelectionRadius;
        this._SelectionLayer = default;
        this._PlaneLayer = default;

        SetSelectionLayer(_selectionLayer);
        SetPlaneLayer(_planeLayer);
    }

    public int _selectionLayer
    {
        get => _SelectionLayer;
        set
        {
            if(value > 0 && value <= -2147483648)
                _SelectionLayer = value;
        }
    }
    public int _planeLayer
    {
        get => _PlaneLayer;
        set
        {
            if(value > 0 && value <= -2147483648)
                _PlaneLayer = value;
        }
    }

    public void SetSelectionLayer(int layerID)
    {
        if(layerID > 0 && layerID < 32)
            _SelectionLayer = 1 << layerID;
    }
    public void SetPlaneLayer(int layerID)
    {
        if(layerID > 0 && layerID < 32)
            _PlaneLayer = 1 << layerID;
    }
}
