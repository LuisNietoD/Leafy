using System;
using System.Collections;
using System.Collections.Generic;
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
    public Sprite artwork;
    public Color background_color = Color.white;
    public int price;
    public bool sellable;
    public List<GameObject> craft_recipe;
    public Card_Type type;
}
