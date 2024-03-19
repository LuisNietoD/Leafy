using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public SpriteRenderer background;
    public SpriteRenderer icon;
    public TextMeshPro type;
    public TextMeshPro cardName;


    public void UpdateCard(ScriptableCard c)
    {
        background.sprite = c.background;
        icon.sprite = c.artwork;
        type.text = c.type.ToString();
        cardName.text = c.name;
    }
}
