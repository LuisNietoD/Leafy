using System;
using Leafy.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class WordHoverDetector : MonoBehaviour
{
    public GameObject cardShow;
    public TextMeshPro title;
    public TextMeshPro textMeshPro;
    private bool isMouseOverText;
    public float offset;
    public TutoDisplayer tuto;
    public LayerMask inGameTextUI;
    public CardDisplay cardDisplay;
    public CursorCard cursorCard;
    public GameObject nextButton;
    private ScriptableCard sc;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, inGameTextUI);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject)
            {
                isMouseOverText = true;
            }
            else
            {
                isMouseOverText = false;
                cursorCard.onWord = false;
            }
        }
        else
        {
            isMouseOverText = false;
            cursorCard.onWord = false;
        }

        
        if (isMouseOverText)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, Camera.main);
            string word = "";
            if(linkIndex != -1)
            {
                word = textMeshPro.textInfo.linkInfo[linkIndex].GetLinkID();
            }

            ScriptableCard card = null;
            if(word != "")
            {
                card = CardList.GetCardByName(word);
            }

            if (linkIndex != -1 && card != null)
            {
                ShowCard(card);
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Camera.main.nearClipPlane;
                mousePosition.x += offset;

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("heyyyyyyyyyyyy");
                    QuestManager.instance.UpdateQuest(5);
                    DisplayTuto(card);
                }

                cardShow.transform.position = mousePosition;
                ShowCard(card);
            }
            else
            {
                cursorCard.onWord = false;
            }
        }
    }

    public int index = 0;

    public void DisplayTuto(ScriptableCard c)
    {
        sc = c;
        title.text = c.name;
        textMeshPro.text = c.recipeText[0];
        cardDisplay.UpdateCard(c);
        index = 1;
        if(c.recipeText.Count > 0)
            nextButton.SetActive(false);
        else
            nextButton.SetActive(true);
        
        //tuto.ChangeTuto(textMeshPro);
    }

    public void Next()
    {
        Debug.Log("test");
        textMeshPro.text = sc.recipeText[index++];
        if (sc.recipeText.Count >= index)
            index = 0;
    }

    public void ShowCard(ScriptableCard c)
    {
        cursorCard.onWord = true;
        cursorCard.EnableCard(c.ID);
    }
}