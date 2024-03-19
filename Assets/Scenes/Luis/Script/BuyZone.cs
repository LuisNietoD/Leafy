using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Manager;
using TMPro;
using UnityEngine;

public class BuyZone : MonoBehaviour
{
    public int price;
    public int actualPrice;
    public GameObject booster;
    public TextMeshPro priceText;
    public LayerMask terrainLayer;


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
            actualPrice = price;
            
            if (Camera.main != null)
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 rayDirection = mouseWorldPosition - Camera.main.transform.position;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, rayDirection, Mathf.Infinity, terrainLayer);

                if (hit.collider != null)
                {
                    Vector3 spawnPoint = hit.point;
                    spawnPoint.z = 0;

                    QuestManager.instance.UpdateQuest(4);
                    Instantiate(booster, spawnPoint, Quaternion.identity);
                }
                else
                {
                    Vector2 nearestPoint = FindNearestPointOnTerrain(Camera.main.transform.position, rayDirection);
                    Vector3 spawnPoint = nearestPoint;
                    spawnPoint.z = 0;

                    QuestManager.instance.UpdateQuest(4);
                    Instantiate(booster, spawnPoint, Quaternion.identity);
                }
            }
        }

        priceText.text = actualPrice.ToString();
    }
    
    private Vector2 FindNearestPointOnTerrain(Vector3 position, Vector3 direction)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 50f, terrainLayer);

        if (colliders.Length > 0)
        {
            Vector2 nearestPoint = colliders[0].ClosestPoint(position);
            float nearestDistance = Vector2.Distance(position, nearestPoint);

            foreach (Collider2D collider in colliders)
            {
                Vector2 point = collider.ClosestPoint(position);
                float distance = Vector2.Distance(position, point);
                
                if (distance < nearestDistance)
                {
                    nearestPoint = point;
                    nearestDistance = distance;
                }
            }

            return nearestPoint;
        }

        // If no terrain collider found, return current position
        return position;
    }
}
