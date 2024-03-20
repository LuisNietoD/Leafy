using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject Btn;
    public GameObject OptionsBtn;
    public GameObject commandes;
    public GameObject audio;
    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    
    
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
