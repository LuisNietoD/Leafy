using System.Collections;
using System.Collections.Generic;
using Leafy.Objects;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CreaturePicker : MonoBehaviour
{
    public static CreaturePicker instance;
    public List<Sprite> creatures;
    public CardUI selectedCreature = null;
    public GameObject renamePanel;
    public TextMeshProUGUI nameText;
    public TMP_InputField nameTextInput;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public Sprite GetCreature()
    {
        return creatures[Random.Range(0, creatures.Count)];
    }

    public void Rename()
    {
        if (selectedCreature != null)
        {
            selectedCreature.card.name = nameText.text;
            selectedCreature.cardName.text = nameText.text;
            renamePanel.SetActive(false);
        }
    }
    
    public void OpenRename(CardUI creature)
    {
        nameTextInput.text = creature.cardName.text;
        nameText.text = creature.cardName.text;
        selectedCreature = creature;
        renamePanel.SetActive(true);
    }
}
