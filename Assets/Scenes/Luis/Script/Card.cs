using System.Collections.Generic;
using RNGNeeds;
using UnityEngine;

namespace Leafy.Data
{
    public class Card
    {
        public string name;
        public string type;
        public Sprite artwork;
        public Color backgroundColor;
        public int ID;
        public bool harvestable;
        public int life;
        public bool shakable;
        public bool storeCard;
        public int inventorySize;
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
        public bool interfaces;
        public List<GameObject> interfaceList = new List<GameObject>();
        public bool transmute;
        public List<TransmuteRecipe> transmuteRecipes;


        public Card(ScriptableCard card)
        {
            name = card.name;
            type = card.type.ToString();
            artwork = card.artwork;
            backgroundColor = card.background_color;
            ID = card.ID;
            harvestable = card.harvestable;
            inventory = card.inventory;
            evolve = card.evolve;
            sellable = card.sellable;
            price = card.price;
            interfaces = card.interfaces;
            transmute = card.transmute;

            if (harvestable)
            {
                infinite = card.infinite;
                life = card.life;
                shakable = card.shakable;
                storeCard = card.storeCard;
                inventorySize = card.inventorySize;
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

            if (interfaces)
                interfaceList = card.interfaceList;

            if (transmute)
            {
                transmuteRecipes = new List<TransmuteRecipe>(card.transmuteList);
            }
        }
    }
}