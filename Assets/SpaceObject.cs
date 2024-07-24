using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    public float rotationSpeed;
    public Vector3 center;
    public int cardID;
    
    private Vector3 spawnPoint;

    private void OnDestroy()
    {
        SpaceObjectList.instance.actual = -1;
    }
}
