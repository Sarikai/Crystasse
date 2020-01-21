using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bridge : MonoBehaviour
{
    [SerializeField]
    private GameObject _capsule = null;
    [SerializeField]
    private LineRenderer _line = null;
    [SerializeField]
    private OffMeshLink _offLink = null;
    [SerializeField]
    private MeshRenderer _renderer = null;
    [SerializeField]
    private Collider _collider = null;

    [SerializeField]
    private int _maxBuildValue = 0;
    [SerializeField]
    AnimationCurve _buildCurve = null;
    [SerializeField]
    private int _buildValue = 0;
    [SerializeField]
    private Crystal _startCrystal = null, _endCrystal = null;

    [SerializeField]
    Vector3 end;
    private byte _teamID = 0;
    private Vector3 _linedir;

    public byte TeamID { get => _teamID; set => _teamID = value; }
    public float PercentDone => (float)_buildValue / (float)_maxBuildValue;

    [SerializeField]
    private Vector3 _lineStart = Vector3.zero, _lineEnd = Vector3.zero;

    private void Awake()
    {
        _line.SetPosition(0, _lineStart);
        _line.SetPosition(1, _lineStart);
        _offLink.activated = false;
        end = _lineStart;
        _linedir = (_lineEnd - _lineStart);
        BridgeList.Add(this);
    }

    public bool ConnectsTo(Crystal c) => (_startCrystal == c || _endCrystal == c);

    public void Show(bool value)
    {
        _collider.enabled = value;
        _renderer.enabled = value;
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

            if(_buildValue > _maxBuildValue)
                _buildValue = _maxBuildValue;

            end = _lineStart + _buildCurve.Evaluate(PercentDone) * _linedir;
            _line.SetPosition(1, end);
        }
    }

    private void OnValidate()
    {
        if(_startCrystal != null)
            _lineStart = _startCrystal.transform.position;
        if(_endCrystal != null)
            _lineEnd = _endCrystal.transform.position;

        _line.SetPosition(0, _lineStart);

        _line.SetPosition(1, _lineEnd);

        if(_startCrystal != null && _endCrystal != null)
        {
            _offLink.startTransform = _startCrystal.transform;
            _offLink.endTransform = _endCrystal.transform;

            var lineDir = _lineEnd - _lineStart;

            _capsule.transform.position = _startCrystal.transform.position + lineDir * 0.5f;
            _capsule.transform.LookAt(_endCrystal.transform);
            _capsule.transform.Rotate(new Vector3(90, 0, 0));
            _capsule.transform.localScale = new Vector3(6f, lineDir.magnitude * 0.5f, 0.1f);
        }
    }
}
