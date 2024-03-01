using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount >= 1)
        {
            Vector3 p = Vector3.zero;
            p.z = -1;
            transform.GetChild(0).localPosition = p;
        }
        
    }
}
