using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class WordHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image waterImage; // Reference to your water image
    private TextMeshProUGUI textMeshPro; // Reference to the TextMeshProUGUI component
    private bool isMouseOverText; // Flag to track whether mouse is over the text
    public float offset;

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
        waterImage.enabled = false; // Make sure to hide image when exiting text area
    }

    void Update()
    {
        if (isMouseOverText)
        {
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(textMeshPro, Input.mousePosition, null);

            if (wordIndex != -1 && textMeshPro.textInfo.wordInfo[wordIndex].GetWord() == "Eau") // Change "Eau" to your word
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Camera.main.nearClipPlane; // Set the z-position to match the camera's near clip plane distance
                mousePosition.x += offset;
                waterImage.transform.position = mousePosition; // Convert screen position to world position
                waterImage.enabled = true;
            }
            else
            {
                waterImage.enabled = false;
            }
        }
    }
}