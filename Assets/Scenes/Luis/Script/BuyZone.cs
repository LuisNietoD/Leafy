using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyZone : MonoBehaviour
{
    public int price;
    public int actualPrice;
    public GameObject booster;
    public TextMeshPro priceText;

    private void Start()
    {
        actualPrice = price;
        priceText.text = actualPrice.ToString();
    }

    public void Buy()
    {
        actualPrice--;
        
        if (actualPrice <= 0)
        {
            Vector3 p = transform.position;
            p.y -= 8;
            Instantiate(booster, p, Quaternion.identity);
            actualPrice = price;
        }

        priceText.text = actualPrice.ToString();
    }
}
