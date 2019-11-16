using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private int _maxBuildValue = 0;
    private int _buildValue = 0;
    private byte _teamID = 0;

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
