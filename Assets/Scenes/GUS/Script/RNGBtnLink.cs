using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class RNGBtnLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public string linkToOpen = "https://rngneeds.com/";
    [SerializeField] private AudioSource Hover;
    [SerializeField] private AudioSource Click;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenLink);
    }
    
    private void OpenLink()
    {
        Click.Play();
        Process.Start(linkToOpen);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        Hover.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}