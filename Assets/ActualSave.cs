using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualSave : MonoBehaviour
{
    public static string fileName = "1";
    private void Awake()
    {
        ES3AutoSaveMgr.Current.settings.path = fileName;
    }
}
