using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Objects;
using RNGNeeds;
using UnityEngine;
using UnityEngine.EventSystems;

public class Booster : MonoBehaviour
{
    public int numberOfPositions = 5;
    public float radius = 4f;
    public GameObject cardsPrefab;
    private int index = 0;
    private List<Vector3> positions = new List<Vector3>();
    public ProbabilityList<ScriptableCard> cards = new ProbabilityList<ScriptableCard>();
    public bool fixedList;
    public List<ScriptableCard> fixedCard = new List<ScriptableCard>();
    public Animator anim;
    public bool isOpen;


    void Start()
    {
        positions = GeneratePositions(transform.position, numberOfPositions, radius);
        cards.PreventRepeat = PreventRepeatMethod.Shuffle;
    } 
    
    public void SpawnCard()
    {
        if (!isOpen)
        {
            isOpen = true;
            anim.Play("OpenPack");
            return;
        }
        GameObject newCard = Instantiate(cardsPrefab, transform.position + positions[index], Quaternion.identity);
        
        if(!fixedList)
            newCard.GetComponent<CardUI>().UpdateCardInfo(new Card(cards.PickValue()));
        else
            newCard.GetComponent<CardUI>().UpdateCardInfo(new Card(fixedCard[index]));
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
            float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float z = 0;

            Vector3 position = new Vector3(x, y, z);
            positions.Add(position);
        }

        return positions;
    }
    
}
