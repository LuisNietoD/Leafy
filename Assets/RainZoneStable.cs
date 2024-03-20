using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Objects;
using UnityEngine;

public class RainZoneStable : MonoBehaviour
{
    public List<GameObject> plants = new List<GameObject>();
    public float waterTime = 3;
    private float elapsedTime = 3;

    public CardUI cardParent;

    private void Start()
    {
        cardParent = transform.parent.GetComponent<CardUI>();
    }

    private void Update()
    {
        if (plants.Count > 0 )
        {
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime < waterTime)
            {
                foreach (GameObject p in plants)
                {
                    p.GetComponent<CardUI>().card.plantRate = 4f;
                }
            }
            else
            {
                foreach (GameObject p in plants)
                {
                    p.GetComponent<CardUI>().card.plantRate = 1f;
                }
            }
            
            if (elapsedTime >= waterTime)
            {
                if (cardParent.card.actualstoredCard > 0)
                {
                    cardParent.card.actualstoredCard--;
                    elapsedTime = 0;
                }
            }
        }
        
        cardParent.slotText.text = cardParent.card.actualstoredCard + "/" + cardParent.card.slotAmount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CardUI cardUI))
        {
            if (cardUI.card.type == "Plant" && cardUI.card.harvestable)
            {
                plants.Add(cardUI.gameObject);
                cardUI.card.plantRate = 4f;
            }
            
            if (cardUI.card.ID == 1)
            {
                if (cardParent.card.actualstoredCard < cardParent.card.slotAmount)
                {
                    Destroy(cardUI.gameObject);
                    cardParent.card.actualstoredCard++;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CardUI cardUI))
        {
            if (cardUI.card.type == "Plant" && cardUI.card.harvestable)
            {
                if (plants.Contains(cardUI.gameObject))
                {
                    plants.Remove(cardUI.gameObject);
                }
                cardUI.card.plantRate = 1f;
            }
        }
    }
}
