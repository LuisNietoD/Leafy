using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Blinking : MonoBehaviour
{
    public GameObject imageObj;
    public TextMeshProUGUI textMeshProObj;
    public float blinkSpeed = 2f;
    public int blinkCount = 2; // Le nombre de clignotements

    private int currentBlink = 0;
    private bool isVisible = true;

    private void Start()
    {
        // Assurez-vous que les objets nécessaires sont assignés dans l'éditeur Unity.
        if (imageObj == null)
        {
            Debug.LogError("Veuillez assigner l'objet Image dans l'éditeur Unity.");
        }

        if (textMeshProObj == null)
        {
            Debug.LogError("Veuillez assigner l'objet TextMeshProUGUI dans l'éditeur Unity.");
        }

        // Lancez la coroutine de clignotement.
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (currentBlink < blinkCount)
        {
            // Déterminez la direction du clignotement (augmentation ou diminution d'alpha).
            float targetAlpha = isVisible ? 0f : 1f;

            if (imageObj != null)
            {
                Image imageComponent = imageObj.GetComponent<Image>();
                if (imageComponent != null)
                {
                    float currentAlphaImage = imageComponent.color.a;
                    currentAlphaImage = Mathf.MoveTowards(currentAlphaImage, targetAlpha, Time.deltaTime * blinkSpeed);
                    Color newColor = imageComponent.color;
                    newColor.a = currentAlphaImage;
                    imageComponent.color = newColor;
                }
            }

            if (textMeshProObj != null)
            {
                float currentAlphaTextMeshPro = textMeshProObj.color.a;
                currentAlphaTextMeshPro = Mathf.MoveTowards(currentAlphaTextMeshPro, targetAlpha, Time.deltaTime * blinkSpeed);
                Color newColor = textMeshProObj.color;
                newColor.a = currentAlphaTextMeshPro;
                textMeshProObj.color = newColor;
            }

            // Si l'alpha atteint la cible, inversez l'état de visibilité.
            if (Mathf.Approximately((imageObj != null ? imageObj.GetComponent<Image>().color.a : 0f), targetAlpha)
                && Mathf.Approximately((textMeshProObj != null ? textMeshProObj.color.a : 0f), targetAlpha))
            {
                isVisible = !isVisible;

                // Si un cycle de clignotement est terminé, incrémentez le compteur.
                if (!isVisible)
                {
                    currentBlink++;
                }
            }

            // Attendez avant de passer à la prochaine itération.
            yield return null;
        }

        // Une fois que les deux clignotements sont terminés, rendez les objets complètement transparents.
        SetObjectsTransparent();

        // Facultatif : Vous pouvez désactiver les objets ou effectuer d'autres actions à ce stade.
    }

    private void SetObjectsTransparent()
    {
        if (imageObj != null)
        {
            Image imageComponent = imageObj.GetComponent<Image>();
            if (imageComponent != null)
            {
                Color newColor = imageComponent.color;
                newColor.a = 0f;
                imageComponent.color = newColor;
            }
        }

        if (textMeshProObj != null)
        {
            Color newColor = textMeshProObj.color;
            newColor.a = 0f;
            textMeshProObj.color = newColor;
        }
    }
}
