using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionBTN : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject Btn;
    public GameObject OptionsBtn;
    public GameObject audio;
    public GameObject commandes;
    private PauseMenu pauseMenu;
    public GameObject pauseMenuUI;
    public GameObject image;
    public Color active;
    public Color normal;
    public ButtonType type;
    public TextMeshProUGUI TextMeshPro;
    [SerializeField] private AudioSource Click;
    [SerializeField] private AudioSource Hover;

    public enum ButtonType
    {
        RESUME, OPTION, MENU, AUDIO, COMMANDES
    }
    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
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
            case ButtonType.RESUME:
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                pauseMenu.gameIsPaused = false;
                Click.Play();
                break;
            case ButtonType.OPTION:
                OptionsBtn.SetActive(true);
                Btn.SetActive(false);   
                Click.Play();
                break;
            case ButtonType.MENU:
                Debug.Log("Menu");
                Click.Play();
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                break;
            case ButtonType.AUDIO:
                Debug.Log("Audio");
                audio.SetActive(true);
                OptionsBtn.SetActive(false);
                Click.Play();
                break;
            case ButtonType.COMMANDES:
                Debug.Log("Commandes");
                commandes.SetActive(true);
                OptionsBtn.SetActive(false);
                Click.Play();
                break;
        }
    }
    
}
