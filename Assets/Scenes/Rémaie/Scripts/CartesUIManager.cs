using UnityEngine;

public class CartesUIManager : MonoBehaviour
{
    public GameObject cartesPanel;

    private void Start()
    {
        // Désactive le panneau au départ
        if (cartesPanel != null)
        {
            cartesPanel.SetActive(false);
        }
    }

    public void AfficherCartesPanel()
    {
        // Active le panneau lorsque la souris entre
        if (cartesPanel != null)
        {
            cartesPanel.SetActive(true);
        }
    }

    public void CacherCartesPanel()
    {
        // Désactive le panneau lorsque la souris quitte
        if (cartesPanel != null)
        {
            cartesPanel.SetActive(false);
        }
    }
}
