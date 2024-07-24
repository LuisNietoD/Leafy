using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public GameObject Btn;
    public GameObject OptionsBtn;
    public GameObject commandes;
    public GameObject audio;
    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        Time.timeScale = 1;
    }
    
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(gameIsPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void Pause()
    {
        Btn.SetActive(true);
        pauseMenuUI.SetActive(true);
        OptionsBtn.SetActive(false);
        commandes.SetActive(false);
        audio.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    
}
