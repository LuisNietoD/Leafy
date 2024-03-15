using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using TMPro;
using UnityEngine;

public class CursorCard : MonoBehaviour
{
    public GameObject fakeCard;
    public SpriteRenderer background;
    public SpriteRenderer icon;
    public TextMeshPro type;
    public TextMeshPro cardName;
    
    public LayerMask UiMask;

    public float offset;
    
    public void EnableCard(int c)
    {
        ScriptableCard card = CardList.GetCardByID(c);
        background.color = card.background_color;
        icon.sprite = card.artwork;
        type.text = card.type.ToString();
        cardName.text = card.name;
        fakeCard.SetActive(true);
    }
    
    public void DisableCard()
    {
        fakeCard.SetActive(false);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, UiMask);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.transform.parent.gameObject.TryGetComponent(out CraftButton craftButton))
            {
                EnableCard(craftButton.craft);

                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.instance.LaunchCraft(craftButton.craft,
                        CardUtils.GetRootCard(craftButton.toFollow.gameObject.GetComponent<CardUI>()));
                    Destroy(craftButton.gameObject);
                }
            }
            else
            {
                DisableCard();
            }
        }
        else
        {
            DisableCard();
        }
        
       
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.x += offset;
        p.z = 0;
        transform.position = p;
    }
}
