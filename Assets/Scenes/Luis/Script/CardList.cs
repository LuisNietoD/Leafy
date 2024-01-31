using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Leafy.Data
{
    public class CardList : MonoBehaviour
    {
        public string scriptableObjectsDirectory = "Assets/Scenes/Luis/Cards";
        private static List<ScriptableCard> Cards = new List<ScriptableCard>();

        public static event Action OnScriptableObjectsLoaded;

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

            string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableCard).Name, new[] { scriptableObjectsDirectory });

            foreach (var guid in guids)
            {
                string cardPath = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableCard card = AssetDatabase.LoadAssetAtPath<ScriptableCard>(cardPath);

                if (card != null)
                {
                    cardList.Add(card);
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

        public static ScriptableCard GetRandomCard()
        {
            Debug.Log(Cards.Count);
            return Cards[Random.Range(0, Cards.Count)];
        }
    }
}

