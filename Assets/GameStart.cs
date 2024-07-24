using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject originalPack;
    
    void Start()
    {
        string filePath = Path.Combine(Application.persistentDataPath, ES3AutoSaveMgr.Current.settings.path);
        if (!File.Exists(filePath))
        {
            GameObject o = Instantiate(originalPack, Vector3.zero, quaternion.identity);
            o.GetComponent<Booster>().fixedList = true;
        }
    }

    
}
