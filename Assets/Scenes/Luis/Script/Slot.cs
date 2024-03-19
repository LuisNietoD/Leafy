using System.Collections;
using System.Collections.Generic;
using Leafy.Objects;
using UnityEngine;

public class Slot : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount >= 1)
        {
            Vector3 p = transform.position;
            p.z = -1;
            if(transform.GetChild(0).childCount > 0)
                transform.GetChild(0).GetComponent<StackParent>().inStack[0].transform.position = p;
        }
        
    }

    public bool TestPlant()
    {
        if (transform.childCount <= 0)
                 return true;
        
        if (transform.GetChild(0).GetComponent<StackParent>().inStack[0].TryGetComponent(out CardUI plant))
        {
            return plant.card.plantIsFull;
        }
        
        return false;
    }
}
