using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Keybind : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI[] buttonLabels = new TextMeshProUGUI[5];

    private string[] playerPrefsKeys = {"CustomKey1", "CustomKey2", "CustomKey3", "CustomKey4", "CustomKey5"};

    void Start()
    {
        for (int i = 0; i < buttonLabels.Length; i++)
        {
            buttonLabels[i].text = PlayerPrefs.GetString(playerPrefsKeys[i]);
        }
    }
    
    void Update()
    {
        for (int i = 0; i < buttonLabels.Length; i++)
        {
            if (buttonLabels[i].text == "Entrer une touche")
            {
                foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(keycode))
                    {
                        buttonLabels[i].text = keycode.ToString();
                        PlayerPrefs.SetString(playerPrefsKeys[i], keycode.ToString());
                        PlayerPrefs.Save();
                    }
                }
            }
        }
    }

    public void ChangeKey(int buttonIndex)
    {
        buttonLabels[buttonIndex].text = "Entrer une touche";
        PlayerPrefs.SetString(playerPrefsKeys[buttonIndex], "Entrer une touche");
        PlayerPrefs.Save();
    }
}