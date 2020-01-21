using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public Area GameArea;
    public int MaxPlayer = 2;
    [Header("Team 1")]
    public Camera CameraT1 = null;
    public Crystal BaseT1 = null;
    [Header("Team 2")]
    public Camera CameraT2 = null;
    public Crystal BaseT2 = null;
}
