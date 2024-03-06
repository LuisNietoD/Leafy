using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using TMPro;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public GameObject infoCanvas;
    public ShowBigCard recipeMenu;
    public GameObject buttonNext;
    public LayerMask cardLayer;

    public string cardName;
    public List<string> recipeList;
    public List<utility> utilityList;
    public bool isRecipe;
    public int index;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, cardLayer);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Info info))
                {
                    isRecipe = true;
                    cardName = info.cardName;
                    recipeList = info.recipes;
                    
                    index = 0;
                    buttonNext.SetActive(recipeList.Count > 1); 
                    infoCanvas.SetActive(true);
                    recipeMenu.ChangeTuto(CardList.GetCardByName(cardName), 0);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, cardLayer);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Info info))
                {
                    isRecipe = false;
                    utilityList = info.utility;
                    
                    
                    //title.text = utilityList[0].cardName;
                    //recipe.text = utilityList[0].recipe;
                    index = 0;
                    buttonNext.SetActive(utilityList.Count > 1); 
                    infoCanvas.SetActive(true);
                    
                }
            }
        }
    }

    public void Next()
    {
        index++;

        if (isRecipe)
        {
            if (index >= recipeList.Count)
                index = 0;
            if (index < 0)
                index = recipeList.Count - 1;
            
            recipeMenu.ChangeTuto(CardList.GetCardByName(cardName), index);
            //recipe.text = recipeList[index];
        }
        else
        {
            if (index >= utilityList.Count)
                index = 0;
            if (index < 0)
                index = utilityList.Count - 1;
            
            //title.text = utilityList[index].cardName;
            //recipe.text = utilityList[index].recipe;
        }
    }

    /*public void Previous()
    {
        index--;
        if(isRecipe)
            recipe.text = recipeList[index];
        else
        {
            title.text = utilityList[index].cardName;
            recipe.text = utilityList[index].recipe;
        }
    }*/
}