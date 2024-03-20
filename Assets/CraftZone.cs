using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using TMPro;
using UnityEngine;

public class CraftZone : MonoBehaviour
{
    public CardButton cardButton;
    public CardDisplay actualCraft;
    public List<CardDisplay> fakecard = new List<CardDisplay>();
    public List<TextMeshPro> textCard = new List<TextMeshPro>();
    public Dictionary<int, int> idAmount = new Dictionary<int, int>();
    public Dictionary<int, int> idActual = new Dictionary<int, int>();
    private void OnEnable()
    {
        idAmount.Clear();
        idActual.Clear();
        actualCraft.UpdateCard(CardList.GetCardByID(cardButton.id));
            
        int index = 0;

        foreach (CardDisplay fc in fakecard)
        {
            fc.gameObject.SetActive(false);
        }
        
        foreach (TextMeshPro t in textCard)
        {
            t.gameObject.SetActive(false);
        }
        
        foreach (int id in cardButton.craft)
        {
            if (idAmount.ContainsKey(id))
            {
                idAmount[id]++;
            }
            else
            {
                idAmount.Add(id, 1);
            }
        }
        
        foreach (int id in cardButton.CardInCraft)
        {
            if (idActual.ContainsKey(id))
            {
                idActual[id]++;
            }
            else
            {
                idActual.Add(id, 1);
            }
        }

        foreach (int id in idAmount.Keys)
        {
            fakecard[index].gameObject.SetActive(true);
            fakecard[index].UpdateCard(CardList.GetCardByID(id));
            textCard[index].gameObject.SetActive(true);
            string s = "";
            if (idActual.ContainsKey(id))
            {
                s = idActual[id] + "/" + idAmount[id];
            }
            else
            {
                s = "0/" + idAmount[id];
            }
            textCard[index].text = s;
            index++;
        }
    }

    public void AddCard(int id)
    {
        foreach (CardDisplay cd in fakecard)
        {
            if (cd.id == id)
            {
                if (idActual.ContainsKey(id))
                {
                    idActual[id]++;
                }
                else
                {
                    idActual.Add(id, 1);
                }
                
                string s = "";
                if (idActual.TryGetValue(id, out int i))
                {
                    s = i + "/" + idAmount[id];
                }
                else
                {
                    s = "0/" + idAmount[id];
                }
                
                textCard[fakecard.IndexOf(cd)].text = s;
            }
        }
    }
}
