using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;

public class StackParent : MonoBehaviour
{
    [SerializeField]
    public List<CardUI> inStack = new List<CardUI>();

    public bool firstSpawn = true;
    public int lastChilCount = 0;
    public GameObject craftButton;
    public GameObject actualButton;

    private void Start()
    {
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

        if(actualButton != null)
            Destroy(actualButton);
        int craft = Craft.GetCraft(CardUtils.GetStackIDList(inStack[0]));
        if (craft >= 0 && inStack[0].ID != 11)
        {
            actualButton = Instantiate(craftButton, transform.position, Quaternion.identity);
            CraftButton cb = actualButton.GetComponent<CraftButton>();
            cb.toFollow = inStack[0].transform;
            cb.craft = craft;
        }
        
        firstSpawn = false;
    }

    private void Update()
    {
        if (lastChilCount != transform.childCount && transform.childCount > 0)
        {
            if(actualButton != null)
                Destroy(actualButton);
            if (inStack.Count > 0)
            {
                if(inStack[0].loader != null)
                    Destroy(inStack[0].loader);
                int craft = Craft.GetCraft(CardUtils.GetStackIDList(inStack[0]));
                if (craft >= 0 && inStack[0].ID != 11)
                {
                    actualButton = Instantiate(craftButton, transform.position, Quaternion.identity);
                    CraftButton cb = actualButton.GetComponent<CraftButton>();
                    cb.toFollow = inStack[0].transform;
                    cb.craft = craft;
                }
            }

            lastChilCount = transform.childCount;
        }
        
        if(transform.childCount <= 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(actualButton != null)
            Destroy(actualButton);
    }
}
