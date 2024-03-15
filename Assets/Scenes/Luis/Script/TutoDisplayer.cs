using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using TMPro;
using UnityEngine;

public enum TutoType
{
    Stack,
    Press
}
public class TutoDisplayer : MonoBehaviour
{
    public GameObject fakeCardPrefab;
    public GameObject cardParent;
    public float stackOffset = 300;
    public float xOffset = 100;
    
    public void ChangeTuto(TextMeshProUGUI recipe)
    {
        foreach (Transform child in cardParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        string word = recipe.text;
        List<string> info = new List<string>();

        int startIndex = word.IndexOf("<link=") + "<link=".Length;
        int endIndex = word.IndexOf(">");
        string itemsText = word.Substring(startIndex, endIndex - startIndex);

        string[] parts = itemsText.Split(',');

        foreach (string part in parts)
        {
            string trimmedPart = part.Trim('"'); // Remove extra quotes
            info.Add(trimmedPart);
        }

        // Output the result
        for (int i = 0; i < info.Count; i++)
        {
            Debug.Log("info[" + i + "]: " + info[i]);
        }

        string type = info[0];

        
        if (type == TutoType.Stack.ToString())
        {
            stackTuto(info);
        }
    }

    private bool right = false;

    public void stackTuto(List<string> info)
    {
        for (int i = 1; i < info.Count; i++)
        {
            string[] recipe = info[i].Split(':');
            for (int j = 0; j < int.Parse(recipe[0]); j++)
            {
                Vector3 p = transform.position;
                p.y -= stackOffset * (i - 1 + j);
                
                GameObject c = Instantiate(fakeCardPrefab, p, Quaternion.identity, cardParent.transform);
                c.GetComponent<FakeCard>().ChangeVisual(CardList.GetCardByName(recipe[1]));
            }
            
        }
    }
}
