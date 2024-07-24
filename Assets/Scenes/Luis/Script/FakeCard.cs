using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FakeCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ScriptableCard card;
    public Image background;
    public Image icon;
    public TextMeshProUGUI type;
    public TextMeshProUGUI nameText;
    public bool bigCard;
    public float offset;
    public ShowBigCard showBigCard;
    
    public void ChangeVisual(ScriptableCard c)
    {
        card = c;
        
       // background.sprite = card.background;
        icon.sprite = card.artwork;
        type.text = card.type.ToString();
        nameText.text = card.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.GetComponent<ShowBigCard>().ShowBig(card);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.GetComponent<ShowBigCard>().HideBig();
    }

    private void Update()
    {
        if (bigCard)
        {
            Vector3 p = Input.mousePosition;
            p.x += offset;
            transform.position = p;

            if (Input.GetMouseButtonDown(0))
            {
                showBigCard.ChangeTuto(card, 0);
            }
        }
    }
}
