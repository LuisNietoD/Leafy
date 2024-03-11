using Leafy.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class WordHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cardShow;
    public TextMeshProUGUI title;
    private TextMeshProUGUI textMeshPro;
    private bool isMouseOverText;
    public float offset;
    public TutoDisplayer tuto;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverText = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverText = false;
        cardShow.SetActive(false);
    }

    void Update()
    {
        if (isMouseOverText)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);
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
                cardShow.GetComponent<FakeCard>().ChangeVisual(card);
                
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Camera.main.nearClipPlane;
                mousePosition.x += offset;

                if (Input.GetMouseButtonDown(0))
                {
                    title.text = card.name;
                    textMeshPro.text = card.recipeText[0];
                    tuto.ChangeTuto(textMeshPro);
                }

                cardShow.transform.position = mousePosition;
                cardShow.SetActive(true);
            }
            else
            {
                cardShow.SetActive(false);
            }
        }
    }
}