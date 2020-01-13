using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private event Action OnBuilt;

    [SerializeField]
    private LineRenderer _line = null;

    [SerializeField]
    private int _maxBuildValue = 0;
    [SerializeField]
    AnimationCurve _buildCurve = null;
    [SerializeField]
    private int _buildValue = 0;

    [SerializeField]
    Vector3 end;
    private byte _teamID = 0;

    public byte TeamID { get => _teamID; set => _teamID = value; }
    public float PercentDone => (float)_buildValue / (float)_maxBuildValue;

    [SerializeField]
    private Vector3 _lineStart = Vector3.zero, _lineEnd = Vector3.zero;

    private void Awake()
    {
        //OnBuilt += () => _line.enabled = true;
        _line.SetPosition(0, _lineStart);
        end = _lineStart;
    }

    float perc = 0;
    private void Update()
    {
        Build(1);
        end = _buildCurve.Evaluate(PercentDone) * _lineEnd;
        _line.SetPosition(1, /*Vector3.Lerp(_lineStart, _lineEnd, PercentDone)*/ end);
    }

    public void Build(byte value)
    {
        Build((int)value);
    }
    public void Build(int value)
    {
        if(_buildValue < _maxBuildValue)
        {
            _buildValue += value;

            if(_buildValue >= _maxBuildValue)
            {
                _buildValue = _maxBuildValue;
                //OnBuilt.Invoke();
            }
        }
    }
}
