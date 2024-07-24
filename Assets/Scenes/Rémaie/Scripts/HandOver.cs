using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardsUIManager cardsUIManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardsUIManager != null)
        {
            cardsUIManager.ShowCardsPanel();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardsUIManager != null)
        {
            cardsUIManager.HideCardsPanel();
        }
    }
}
