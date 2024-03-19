using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public TextMeshProUGUI title;
    public TextMeshProUGUI questText;
    private bool isMouseOverText;
    public WordHoverDetector wordHoverDetector;
    public GameObject tutoObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverText = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverText = false;
    }

    void Update()
    {
        if (isMouseOverText)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(questText, Input.mousePosition, null);
            string word = "";
            if (linkIndex != -1)
            {
                word = questText.textInfo.linkInfo[linkIndex].GetLinkID();
                Debug.Log(word);
            }

            ScriptableCard card = null;
            if (word != "")
            {
                card = CardList.GetCardByName(word);
            }

            if (linkIndex != -1 && card != null)
            {
                Debug.Log(card.recipeText[0]);
                if (Input.GetMouseButtonDown(0))
                {
                    wordHoverDetector.DisplayTuto(card);
                    tutoObject.SetActive(true);
                }
            }
        }
    }

    public void ChangeActualQuest()
    {
        QuestManager.instance.ChangeQuest();
    }
}
