using System.Collections.Generic;
using RNGNeeds;
using UnityEngine;

namespace Leafy.Data
{
    public class Card
    {
        public string name;
        public Sprite artwork;
        public Color backgroundColor;
        public int ID;
        public bool harvestable;
        public int life;
        public bool shakable;
        public bool infinite;
        public ProbabilityList<ScriptableCard> drop;
        public List<int> activators;
        public bool inventory;
        public int slots;

        public Card(ScriptableCard card)
        {
            name = card.name;
            artwork = card.artwork;
            backgroundColor = card.background_color;
            ID = card.ID;
            harvestable = card.harvestable;
            inventory = card.inventory;
            
            if (harvestable)
            {
                infinite = card.infinite;
                life = card.life;
                shakable = card.shakable;
                drop = card.drop;
                activators = card.activators;
            }

            if (inventory)
            {
                slots = card.slot;
            }
        }
    }
}