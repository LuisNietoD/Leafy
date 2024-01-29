using UnityEngine;

public class CardsUIManager : MonoBehaviour
{
    public GameObject cardsPanel;

    private void Start()
    {
        // D�sactive le panneau au d�part
        if (cardsPanel != null)
        {
            cardsPanel.SetActive(false);
        }
    }

    public void ShowCardsPanel()
    {
        Debug.Log("ShowCardsPanel");
        // Active le panneau lorsque la souris entre
        if (cardsPanel != null)
        {
            cardsPanel.SetActive(true);
        }
    }

    public void HideCardsPanel()
    {
        Debug.Log("HideCardsPanel");
        // D�sactive le panneau lorsque la souris quitte
        if (cardsPanel != null)
        {
            cardsPanel.SetActive(false);
        }
    }
}
