using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Objects;
using UnityEngine;

public class RainZone : MonoBehaviour
{
    public List<GameObject> plants = new List<GameObject>();

    private void Update()
    {
        if (plants.Count > 0)
        {
            foreach (GameObject p in plants)
            {
                Debug.Log(p);
                p.GetComponent<CardUI>().card.plantRate = 3f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.gameObject.TryGetComponent(out CardUI cardUI))
        {
            if (cardUI.card.type == "Plant" && cardUI.card.harvestable)
            {
                plants.Add(cardUI.gameObject);
                cardUI.card.plantRate = 3f;
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
