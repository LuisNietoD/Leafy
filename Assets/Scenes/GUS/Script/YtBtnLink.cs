using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class YtBtnLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public string linkToOpen = "https://www.youtube.com/watch?v=0T-6SrF9oOc";
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