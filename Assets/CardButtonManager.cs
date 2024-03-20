using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using UnityEngine;
using UnityEngine.Events;

public class CardButtonManager : MonoBehaviour
{
    public LayerMask hudLayer;
    public CardButton cardButton;
    public GameObject craftPage;
    public GameObject recipeList;
    public static event UnityAction<ScriptableCard> OnClickCard;

    // Update is called once per frame
    void Update()
    {
        if (recipeList.activeSelf)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, hudLayer);

            if (hit.collider != null && Input.GetMouseButtonDown(0))
            {
                if (hit.transform.TryGetComponent(out CardButton cb))
                {
                    cardButton = cb;
                    craftPage.GetComponent<CraftZone>().cardButton = cardButton;
                    craftPage.SetActive(true);
                    recipeList.SetActive(false);
                }
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, hudLayer);

            if (hit.collider != null && Input.GetMouseButtonDown(0))
            {
                if (hit.transform.TryGetComponent(out CardDisplay cb))
                {
                    if (OnClickCard != null) OnClickCard.Invoke(CardList.GetCardByID(cb.id));
                }
            }
        }
    }
}
