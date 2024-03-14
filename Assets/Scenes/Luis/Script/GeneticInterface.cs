using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;

namespace Leafy.Objects
{
    public class GeneticInterface : CardGameInterface
    {
        public GameObject first;
        public GameObject second;
    
        public override void InterfaceAction()
        {
            if (first.transform.childCount > 0 && second.transform.childCount > 0)
            {
                List<int> cards = new List<int>();
                cards.Add(first.transform.GetChild(0).GetComponent<CardUI>().ID);
                cards.Add(second.transform.GetChild(0).GetComponent<CardUI>().ID);

                int result = Craft.GetGenCraft(cards);

                if (result != -1)
                {
                    Vector3 p = transform.position;
                    p.y -= 3.5f;
                    GameManager.instance.SpawnCard(p, result);
                    Destroy(first.transform.GetChild(0).gameObject);
                    Destroy(second.transform.GetChild(0).gameObject);
                }
                else
                {
                    cards.Clear();
                    List<CardUI> c = new List<CardUI>();
                    c.Add(first.transform.GetChild(0).GetComponent<CardUI>());
                    c.Add(second.transform.GetChild(0).GetComponent<CardUI>());
                    bool plant = false;

                    foreach (CardUI card in c)
                    {
                        if (card.card.type == Card_Type.Plant.ToString() && card.card.harvestable)
                        {
                            plant = true;
                            cards.Add(card.ID);
                        }
                    }

                    if (plant && cards.Count >= 2)
                    {
                        if (cards[0] == cards[1])
                        {
                            CardUI newPlant = first.transform.GetChild(0).GetComponent<CardUI>();
                            CardUI oldPlant = second.transform.GetChild(0).GetComponent<CardUI>();

                            if(newPlant.card.productivityLevel >= 5 || oldPlant.card.productivityLevel >= 5)
                                return;
                            newPlant.card.productivityLevel += oldPlant.card.productivityLevel + 1;
                            newPlant.card.productivityLevel = Mathf.Clamp(newPlant.card.productivityLevel, 0, 5);

                            newPlant.card.rateLevel += oldPlant.card.rateLevel;
                            newPlant.card.rateLevel = Mathf.Clamp(newPlant.card.rateLevel, 0, 5); 

                            newPlant.card.storageLevel += oldPlant.card.storageLevel;
                            newPlant.card.storageLevel = Mathf.Clamp(newPlant.card.storageLevel, 0, 5);

                            
                            Vector3 p = transform.position;
                            p.y -= 3.5f;
                            newPlant.transform.position = p;
                            newPlant.transform.parent = null;
                            Destroy(second.transform.GetChild(0).gameObject);
                            
                        }
                        return;
                    }

                    if (plant)
                    {
                        if (cards[0] == c[0].ID)
                        {
                            cards.Add(c[1].ID);
                        }
                        else
                        {
                            cards.Add(c[0].ID);
                        }

                        if (cards.Contains(7))
                        {
                            CardUI newPlant = c.Find(ui => ui.ID != 7);
                            CardUI pot = c.Find(ui => ui.ID == 7);
                            
                            if(newPlant.card.storageLevel >= 5)
                                return;
                            newPlant.card.storageLevel += 1;
                            newPlant.card.storageLevel = Mathf.Clamp(newPlant.card.storageLevel, 0, 5);

                            
                            Vector3 p = transform.position;
                            p.y -= 3.5f;
                            newPlant.transform.position = p;
                            newPlant.transform.parent = null;
                            Destroy(pot.gameObject);
                        }
                        else if (cards.Contains(79))
                        {
                            CardUI newPlant = c.Find(ui => ui.ID != 79);
                            CardUI compost = c.Find(ui => ui.ID == 79);
                            
                            if(newPlant.card.rateLevel >= 5)
                                return;
                            newPlant.card.rateLevel += 1;
                            newPlant.card.rateLevel = Mathf.Clamp(newPlant.card.rateLevel, 0, 5);

                            
                            Vector3 p = transform.position;
                            p.y -= 3.5f;
                            newPlant.transform.position = p;
                            newPlant.transform.parent = null;
                            Destroy(compost.gameObject);
                        }
                    }

                }
                
            }
        }
    }
}
