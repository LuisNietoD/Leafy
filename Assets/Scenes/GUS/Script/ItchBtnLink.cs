using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class ItchBtnLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string linkToOpen = "https://novaeka.itch.io/leafy";
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenLink);
    }
    
    private void OpenLink()
    {
        Process.Start(linkToOpen);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}