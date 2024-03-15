using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;


public class ChangeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject bloc;
    public GameObject Delete;
    public GameObject image;
    public Color active;
    public Color normal;
    public ButtonType type;
    public TextMeshProUGUI TextMeshPro;
    [SerializeField] private AudioSource Click;

    public enum ButtonType
    {
        PLAY, DELETE
    }

    private void Update()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "SaveFile.es3");
        if (File.Exists(filePath))
        {
            bloc.SetActive(false);
        }
        else
        {
            bloc.SetActive(true);
        }
    }

    public void DeleteSaveFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "SaveFile.es3");

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Delete.SetActive(false);
                Debug.Log("Fichier supprimé avec succès !");
            }
            catch (IOException e)
            {
                Debug.LogError("Erreur lors de la suppression du fichier : " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Le fichier n'existe pas à l'emplacement spécifié.");
        }
    }
    public void Load()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().color=active;
            gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize=90;
            image.SetActive(true);
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
            case ButtonType.PLAY:
                Debug.Log("Play");
                SceneManager.LoadScene(2, LoadSceneMode.Single);
                Click.Play();
            break;
            case ButtonType.DELETE:
                Debug.Log("Delete");
                Delete.SetActive(true);
                Click.Play(); 
                break;
        }
    }
    
}
