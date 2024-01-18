using UnityEngine;

public class CartesUIManager : MonoBehaviour
{
    public GameObject cartesPanel;

    private void Start()
    {
        // D�sactive le panneau au d�part
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
        // D�sactive le panneau lorsque la souris quitte
        if (cartesPanel != null)
        {
            cartesPanel.SetActive(false);
        }
    }
}
