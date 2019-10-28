using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private int _maxBuildValue;
    private int _buildValue;
    private byte _teamID;

    public byte TeamID { get => _teamID; set => _teamID = value; }
    public float PercentDone => _maxBuildValue / _buildValue;

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
                _buildValue = _maxBuildValue;
        }
    }
}
