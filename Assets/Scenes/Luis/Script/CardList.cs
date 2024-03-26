using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Leafy.Data
{
    public class CardList : MonoBehaviour
    {
        public static List<ScriptableCard> Cards = new List<ScriptableCard>();

        public static event Action OnScriptableObjectsLoaded;

       
        void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("CardList");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }
        

        void Start()
        { 
            StartCoroutine(LoadScriptableObjectsCoroutine());
        }

        IEnumerator LoadScriptableObjectsCoroutine()
        {
            yield return StartCoroutine(LoadScriptableObjects());

            // Notify subscribers that scriptable objects are loaded
            OnScriptableObjectsLoaded?.Invoke();
        }

        IEnumerator LoadScriptableObjects()
        {
            List<ScriptableCard> cardList = new List<ScriptableCard>();

            // Assuming your ScriptableCard assets are located in a "Resources" folder
            Object[] loadedObjects = Resources.LoadAll("Cards", typeof(ScriptableCard));

            foreach (var obj in loadedObjects)
            {
                if (obj is ScriptableCard)
                {
                    ScriptableCard card = (ScriptableCard)obj;
                    cardList.Add(card);
                    if (card.craftable)
                    {
                        foreach (CraftRecipe recipe in card.recipes)
                        {
                            switch (recipe.machine)
                            {
                                case Machine.None:
                                    Craft.list.Add(card.ID, recipe.recipe);
                                    break;
                                case Machine.Press:
                                    Craft.press.Add(card.ID, recipe.recipe);
                                    break;
                                case Machine.Genetic:
                                    Craft.gen.Add(card.ID, recipe.recipe);
                                    break;
                                case Machine.Mixer:
                                    Craft.mixer.Add(card.ID, recipe.recipe);
                                    break;
                                case Machine.Assembleur:
                                    Craft.assembleur.Add(card.ID, recipe.recipe);
                                    break;
                            }
                        }
                    }
                        
                }
            }
            
            Cards = cardList;

            // You can yield here if you want to wait for something else.
            // For now, let's just return null to finish the coroutine immediately.
            yield return null;
        }
        
        // Get Card by ID
        public static ScriptableCard GetCardByID(int targetID)
        {
            return Cards.Find(card => card.ID == targetID);
        }
        
        public static ScriptableCard GetCardByName(string name)
        {
            return Cards.Find(card => card.name.ToLower() == name.ToLower());
        }

        public static ScriptableCard GetRandomCard()
        {
            Debug.Log(Cards.Count);
            return Cards[Random.Range(0, Cards.Count)];
        }

        public static List<ScriptableCard> GetCardList(int id, int count)
        {
            List<ScriptableCard> cards = new List<ScriptableCard>();

            for (int i = 0; i < count; i++)
            {
                cards.Add(GetCardByID(id));
            }

            return cards;
        }
        
        public static List<ScriptableCard> GetRandomCardList(int count)
        {
            List<ScriptableCard> cards = new List<ScriptableCard>();

            for (int i = 0; i < count; i++)
            {
                cards.Add(GetRandomCard());
            }

            return cards;
        }
    }
}

