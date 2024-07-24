using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using TMPro;
using UnityEngine;

public class CraftButton : MonoBehaviour
{
    public Transform toFollow;
    public int craft;
    
    private void Start()
    {
        transform.position = toFollow.position;
    }
    
    void Update()
    {
        transform.position = toFollow.position;
    }
}
