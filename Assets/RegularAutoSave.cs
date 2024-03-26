using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularAutoSave : MonoBehaviour
{
    public float saveRateSeconds = 30;
    private float elapsedTime = 0;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= saveRateSeconds)
        {
            elapsedTime = 0;
            ES3AutoSaveMgr._current.Save();
        }
    }

    public void Save()
    {
        ES3AutoSaveMgr._current.Save();
    }
}
