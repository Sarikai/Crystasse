using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public Bridge[] Bridges = null;
    public Area GameArea;
    [Header("Team 1")]
    public Camera CameraT1 = null;
    public Crystal BaseT1 = null;
    public Crystal[] CrystalsT1 = null;
    [Header("Team 2")]
    public Camera CameraT2 = null;
    public Crystal BaseT2 = null;
    public Crystal[] CrystalsT2 = null;

}
