using UnityEngine;

public class CardsUIManager : MonoBehaviour
{
    public GameObject cardsPanel;

    private void Start()
    {
        // Désactive le panneau au départ
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
        // Désactive le panneau lorsque la souris quitte
        if (cardsPanel != null)
        {
            cardsPanel.SetActive(false);
        }
    }
}
