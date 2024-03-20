using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Objects;
using UnityEngine;

public class WirelessCharge : MonoBehaviour
{
    public List<GameObject> machine = new List<GameObject>();
    public CardUI cardParent;

    private void Start()
    {
        cardParent = transform.parent.GetComponent<CardUI>();
    }

    private void Update()
    {
        if (machine.Count > 0)
        {
            foreach (GameObject p in machine)
            {
                CardUI c = p.GetComponent<CardUI>();

                if (c.card.actualEnergy < c.card.energyCost && cardParent.card.actualEnergy > 0)
                {
                    cardParent.card.actualEnergy--;
                    c.card.actualEnergy++;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.gameObject.TryGetComponent(out CardUI cardUI))
        {
            if (cardUI.card.requiereEnergy)
            {
                machine.Add(cardUI.gameObject);
            }

            if (CardUtils.GetLastCard(cardUI).card.ID == 39)
            {
                if (cardParent.card.actualEnergy < cardParent.card.maxEnergy)
                {
                    Destroy(CardUtils.GetLastCard(cardUI).gameObject);
                    cardParent.card.actualEnergy++;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CardUI cardUI))
        {
            if (cardUI.card.requiereEnergy)
            {
                if (machine.Contains(cardUI.gameObject))
                {
                    machine.Remove(cardUI.gameObject);
                }
            }
        }
    }
}
