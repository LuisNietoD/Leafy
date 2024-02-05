using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Objects;
using RNGNeeds;
using UnityEngine;


public enum Card_Type
{
    energies,
    resources,
    interaction,
    activator,
    storage,
    interfaceCard,
    autoCard,
    tools
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
    
    [Header("Sell information")]
    public int price;

    [Header("Craft information")]
    public float timeToCraft;
    
    [Header("Harvest information")]
    public bool infinite;
    public int life;
    public bool shakable;
    public ProbabilityList<ScriptableCard> drop;
    public List<int> activators;

    [Header("Inventory information")] 
    public int slot;
    
    public Texture2D GetCardIcon()
    {
        return artwork.texture;
    }
}
