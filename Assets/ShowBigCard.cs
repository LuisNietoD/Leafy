using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowBigCard : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI textMeshPro;
    public TutoDisplayer tuto;
    public WordHoverDetector wordDetector;
    public FakeCard bigCard;

    public void ShowBig(ScriptableCard c)
    {
        bigCard.gameObject.SetActive(true);
        bigCard.ChangeVisual(c);
    }

    public void HideBig()
    {
        bigCard.gameObject.SetActive(false);
    }

    public void ChangeTuto(ScriptableCard c, int index)
    {
        title.text = c.name;
        textMeshPro.text = c.recipeText[index];
        tuto.ChangeTuto(textMeshPro);
    }
}
