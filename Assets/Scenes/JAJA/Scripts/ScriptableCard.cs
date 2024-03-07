using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Objects;
using RNGNeeds;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


public enum Card_Type
{
    Energy,
    Resource,
    Harvestable,
    Activator,
    Storage,
    Plant,
    Standalone,
    Tools
}

public enum Machine
{
    None,
    Press,
    Genetic
}

[Serializable]
public class CraftRecipe
{
    public Machine machine;
    public List<int> recipe;
}

[Serializable]
public class TransmuteRecipe
{
    public List<int> recipe;
    public int resultAmount;
    public ProbabilityList<ScriptableCard> result;
}

[Serializable]
public class SavoirList
{
    public int pointDeSavoir;
    public List<int> result;
}

[CreateAssetMenuAttribute(fileName = "Scriptable Card", menuName = "My Game/ Scriptable Card")]
public class ScriptableCard : ScriptableObject
{
    public new string name;
    public int ID;
    public Sprite artwork;
    public Sprite background;
    public Color background_color = Color.white;
    public Card_Type type;
    public bool sellable;
    public bool craftable;
    public bool harvestable;
    public bool inventory;
    public bool evolve;
    public bool interfaces;
    public bool transmute;
    public bool activableInterface;
    public bool customBehavior;
    public bool machineSavoir;

    [TextArea(1,20)]
    public List<string> recipeText;
    public List<utility> utilities;
    
    [Header("Sell information")]
    public int price;

    [Header("Craft information")]
    public float timeToCraft = 2;
    
    [Header("Harvest information")]
    public bool infinite;
    public int life;
    public bool storeCard;
    public int inventorySize;
    public bool shakable;
    public float harvestTime;
    public ProbabilityList<ScriptableCard> drop;
    public List<int> activators;

    [Header("Inventory information")] 
    public int slotAmount;
    public Card_Type slotType;
    public int preciseCardID;
    public bool preciseCard;

    [Header("Recipe information")] public List<CraftRecipe> recipes = new List<CraftRecipe>();
    
    [Header("Evolve information")] 
    public float evolveTime;
    public ProbabilityList<ScriptableCard> evolutions;
    
    [Header("Interfaces information")] 
    public List<GameObject> interfaceList = new List<GameObject>();

    [Header("Transmute information")] 
    public bool requireEnergy;
    public int maxEnergyStored;
    public int energyPerCraft;
    public List<TransmuteRecipe> transmuteList;

    [Header("Machine a savoir")] public List<SavoirList> savoirList;
    [Header("Custom Behavior")] public string behavior;
    
    public Texture2D GetCardIcon()
    {
        return artwork.texture;
    }
}
