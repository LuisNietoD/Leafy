using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;

public class StackParent : MonoBehaviour
{
    [SerializeField]
    public List<CardUI> inStack = new List<CardUI>();

    public bool firstSpawn = true;

    private void Start()
    {
        /*if(transform.childCount <= 0)
            Destroy(gameObject);
*/
        //Rearrange parent based on list
        if (!firstSpawn)
        {
            inStack.Clear();
            if(transform.childCount <= 0)
                Destroy(gameObject);
            foreach (Transform c in transform)
            {
                inStack.Add(c.gameObject.GetComponent<CardUI>());
            }
            
            if(inStack.Count <= 0)
                Destroy(gameObject);
        }
        
        
        foreach (var c in inStack)
        {
            c.SetParent(null);
        }

        if (inStack.Count > 0)
        {
            inStack[0].SetParent(null);
            inStack[0].ChangeID(GameManager.instance.ID++);
            for (int i = 1; i < inStack.Count; i++)
            {
                inStack[i].SetParent(inStack[i - 1]);
                inStack[i].ChangeID(GameManager.instance.ID++);
            }

            if (inStack.Count > 1)
                inStack[0].child = inStack[1];
        }

        firstSpawn = false;
    }
}
