using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Objects;
using UnityEngine;
using UnityEngine.EventSystems;

public class Booster : MonoBehaviour
{
    public int numberOfPositions = 5;
    public float radius = 4f;
    public GameObject cardsPrefab;
    private int index = 0;
    private List<Vector3> positions = new List<Vector3>();


    void Start() => positions = GeneratePositions(transform.position, numberOfPositions, radius);
    
    public void SpawnCard()
    {
        GameObject newCard = Instantiate(cardsPrefab, positions[index], Quaternion.identity);
        newCard.GetComponent<CardUI>().UpdateCardInfo(new Card(CardList.GetRandomCard()));
        index++;
        if(index >= numberOfPositions)
            Destroy(gameObject);
    }
    
    
    List<Vector3> GeneratePositions(Vector3 center, int count, float radius)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i;
            float x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = center.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float z = center.z;

            Vector3 position = new Vector3(x, y, z);
            positions.Add(position);
        }

        return positions;
    }
    
}
