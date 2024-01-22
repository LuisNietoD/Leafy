using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Leafy.Data
{
    public class CardList : MonoBehaviour
    {
        public string scriptableObjectsDirectory = "Assets/Scenes/Luis/Cards";
        private static List<ScriptableCard> Cards = new List<ScriptableCard>();

        void Start()
        {
            Cards = LoadScriptableObjects();
        }

        List<ScriptableCard> LoadScriptableObjects()
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
            return cardList;
        }
        
        // Get Card by ID
        public static ScriptableCard GetCardByID(int targetID)
        {
            return Cards.Find(card => card.ID == targetID);
        }
    }
}

