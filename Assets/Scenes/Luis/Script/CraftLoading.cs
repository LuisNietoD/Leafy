using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;

public class CraftLoading : MonoBehaviour
{
    public List<CardUI> stack = new List<CardUI>();
    public ScriptableCard drop;
    public float timeToCraft = 1;
    public float elapsed = 0;
    public SpriteRenderer loadImage;
    public bool destroyStack;
    public bool multipleCards;
    public List<int> toCraft = new List<int>();
    public CardUI parent;
    
    private void Start()
    {
        loadImage.material = new Material(loadImage.material);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        loadImage.material.SetFloat("_RadialClip", Mathf.Lerp(360, 0, Mathf.Clamp01(elapsed/timeToCraft)));
        

        if (elapsed >= timeToCraft)
        {
            
            Vector3 pos = stack[0].transform.position;
            pos.x += 3;
            if (multipleCards)
            {
                GameManager.instance.SpawnStackPrecise(pos, toCraft);
            }
            else
            {
                GameManager.instance.SpawnCard(pos, drop.ID);
            }

            if(parent != null)
                if (parent.card.requiereEnergy)
                    parent.card.actualEnergy -= parent.card.energyCost;

            foreach (CardUI card in stack)
            {
                if (destroyStack)
                {
                    if(card.child != null) 
                        card.child.SetParent(card.parent);
                    Destroy(card.gameObject);
                }
                else
                    card.ReduceLife();
                //Destroy(card.gameObject);
            }

            elapsed = 0;
            if (destroyStack)
            {
                Destroy(gameObject);
            }
        }
    }
}
