using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject gameSave;
    public GameObject image;
    public Color active;
    public Color normal;
    public ButtonType type;
    public TextMeshProUGUI TextMeshPro;
    [SerializeField] private AudioSource Click;
    [SerializeField] private AudioSource Hover;

    public enum ButtonType
    {
        JOUER, CREDITS, QUITTER, RETRY, MENU
    }

    private void OnEnable()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color=normal;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize=70;
        image.SetActive(false);
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
        switch (type)
        {
            case ButtonType.JOUER:
                Debug.Log("Jouer");
                gameSave.SetActive(true);
                //SceneManager.LoadScene(2, LoadSceneMode.Single);
                Click.Play();
            break;
            case ButtonType.CREDITS:
                Debug.Log("Credits");
                SceneManager.LoadScene(1, LoadSceneMode.Single);
                Click.Play();
            break;
            case ButtonType.QUITTER:
                Debug.Log("Quitter");
                Click.Play();
                Application.Quit();
            break;
            case ButtonType.RETRY:
                Debug.Log("Jouer");
                SceneManager.LoadScene(3, LoadSceneMode.Single);
                Click.Play();
            break;
            case ButtonType.MENU:
                Debug.Log("Menu");
                Click.Play();
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            break;
        }
    }
    
}
