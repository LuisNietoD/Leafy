using System.Collections;
using System.Collections.Generic;
using Leafy.Manager;
using UnityEngine;

public class RecycleZone : MonoBehaviour
{
    public LayerMask terrainLayer;
    public int stackType = 1;
    public int value = 0;

    public void Recycle(int value)
    {
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
                //spawnPoint.x += 5;

                GameManager.instance.SpawnStack(spawnPoint, stackType, value);
            }
            else
            {
                Vector2 nearestPoint = FindNearestPointOnTerrain(Camera.main.transform.position, rayDirection);
                Vector3 spawnPoint = nearestPoint;
                spawnPoint.z = 0;
                //spawnPoint.x += 5;

                GameManager.instance.SpawnStack(spawnPoint, stackType, value);
            }
        }
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
