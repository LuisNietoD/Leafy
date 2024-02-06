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
        public float harvestTime;
        public ProbabilityList<ScriptableCard> drop;
        public List<int> activators;
        public bool inventory;
        public int slots;
        public bool evolve;
        public float evolveTime;
        public ProbabilityList<ScriptableCard> evolutions;
        public bool sellable;
        public int price;

        public Card(ScriptableCard card)
        {
            name = card.name;
            artwork = card.artwork;
            backgroundColor = card.background_color;
            ID = card.ID;
            harvestable = card.harvestable;
            inventory = card.inventory;
            evolve = card.evolve;
            sellable = card.sellable;
            price = card.price;

            if (harvestable)
            {
                infinite = card.infinite;
                life = card.life;
                shakable = card.shakable;
                harvestTime = card.harvestTime;
                drop = card.drop;
                activators = card.activators;
            }

            if (inventory)
            {
                slots = card.slot;
            }

            if (evolve)
            {
                evolveTime = card.evolveTime;
                evolutions = card.evolutions;
            }
        }
    }
}