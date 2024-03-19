using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;


public class OpenSave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
        
    public GameObject image;
    public TextMeshProUGUI TextMeshPro;
    public Color active;
    public Color normal;
    public ButtonType type;
    private bool save1,save2,save3;
    [SerializeField] private AudioSource Hover;
    [SerializeField] private AudioSource Click;
    public enum ButtonType
    {
        SAVE1,SAVE2,SAVE3
    }
    
    public string saveName;

    private void OnEnable()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color=normal;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize=70;
        image.SetActive(false);
    }
    private void Start()
    {
        save1 = false;
        save2 = false;
        save3 = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color=active;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize=90;
        image.SetActive(true);
        Hover.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color=normal;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize=70;
        image.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
                ActualSave.fileName = saveName;
                Click.Play(); 
    }
}

