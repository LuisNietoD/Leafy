using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;

public class CardButton : MonoBehaviour
{
    public List<int> craft = new List<int>();
    public List<int> remainCardForCraft = new List<int>();
    public List<int> CardInCraft = new List<int>();
    public CraftZone craftZone;
    public int id;

    private void Start()
    {
        craft = Craft.list[id];
        remainCardForCraft = new List<int>(craft);
    }
    
    

    public void PutCardinCraft(List<CardUI> stack)
    {
        List<CardUI> recenter = new List<CardUI>();
        foreach (CardUI c in stack)
        {
            if (remainCardForCraft.Contains(c.card.ID))
            {
                remainCardForCraft.Remove(c.card.ID);
                CardInCraft.Add(c.card.ID);
                Destroy(c.gameObject);
            }
            else
            {
                recenter.Add(c);
            }
        }
        
        if(recenter.Count > 0)
            recenter[0].transform.position = recenter[0].CameraCenterToPoint();
    }
}
