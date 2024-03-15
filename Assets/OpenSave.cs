using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSave : MonoBehaviour
{
    public string saveName;

    public void LoadSave()
    {
        ActualSave.fileName = saveName;
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}

