using System;
using System.Collections.Generic;
using Leafy.Objects;
using UnityEngine;

namespace Leafy.Data
{
    [Serializable]
    public class utility
    {
        public string cardName;
        public string recipe;
    }
    
    public class Info: MonoBehaviour
    {
        public string cardName;
        public List<string> recipes = new List<string>();
        public List<utility> utility = new List<utility>();
        private void Start()
        {
            if (TryGetComponent(out CardUI cardUI))
            {
                cardName = cardUI.card.name;
                recipes = cardUI.card.recipes;
                utility = cardUI.card.utilities;
            }
        }
    }
}