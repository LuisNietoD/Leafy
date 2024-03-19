using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowBigCard : MonoBehaviour
{
    public TextMeshPro title;
    public TextMeshPro textMeshPro;
    public TutoDisplayer tuto;
    public WordHoverDetector wordDetector;
    public FakeCard bigCard;
    public CardDisplay cardDisplay;

    public void ShowBig(ScriptableCard c)
    {
        cardDisplay.gameObject.SetActive(true);
        bigCard.ChangeVisual(c);
    }

    public void HideBig()
    {
        cardDisplay.gameObject.SetActive(false);
    }

    public void ChangeTuto(ScriptableCard c, int index)
    {
        title.text = c.name;
        textMeshPro.text = c.recipeText[index];
        cardDisplay.UpdateCard(c);
        //tuto.ChangeTuto(textMeshPro);
    }
}
