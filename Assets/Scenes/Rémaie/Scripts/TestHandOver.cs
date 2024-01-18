using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CartesUIManager cartesUIManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cartesUIManager != null)
        {
            cartesUIManager.AfficherCartesPanel();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cartesUIManager != null)
        {
            cartesUIManager.CacherCartesPanel();
        }
    }
}
