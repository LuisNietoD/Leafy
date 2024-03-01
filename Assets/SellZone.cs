using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;

public class SellZone : MonoBehaviour
{
    public void Sell(int value)
    {
        Vector3 p = transform.position;
        p.y -= 4;
        GameManager.instance.SpawnStack(p, 1, value);
    }
}
