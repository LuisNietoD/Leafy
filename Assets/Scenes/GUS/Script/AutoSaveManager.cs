using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using Unity.VisualScripting;

public class AutoSaveManager : MonoBehaviour
{
    private float autoSaveInterval = 3f; 
    private float timer = 0f;

    public List<Transform> objectsToSave;

    private void Start()
    {
        
        LoadGame();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= autoSaveInterval)
        {
            SaveGame();
            timer = 0f;
        }
    }

    private void SaveGame()
    {
        Debug.Log("Saving game...");
        string savePath = Application.persistentDataPath + "/autosave.sav";

        List<string> saveDataList = new();
        
        
        foreach (Transform objTransform in objectsToSave)
        {
            SaveData saveData = new SaveData();
            saveData.position = objTransform.position;
            string json = saveData.Serialize().json;
            saveDataList.Add(json);
            
            //Debug.Log("Saved position: " + saveData.position);
            Debug.Log(json);
        }

        // Convertit les données en chaîne JSON
        string jsonData = string.Join(',', saveDataList);
        Debug.Log("Saved Data Liste : " + jsonData);
        
        
        // Écrit les données dans le fichier
        File.WriteAllText(savePath, jsonData);
    }

    private void LoadGame()
    {
        string savePath = Application.persistentDataPath + "/autosave.sav";
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            List<SaveData> saveDataList = JsonUtility.FromJson<List<SaveData>>(jsonData);

            // Applique les positions sauvegardées aux objets dans la liste
            for (int i = 0; i < Mathf.Min(objectsToSave.Count, saveDataList.Count); i++)
            {
                objectsToSave[i].position = saveDataList[i].position;
            }
        }
    }
}

[System.Serializable]
public class SaveData
{
    
    public Vector3 position;
}
