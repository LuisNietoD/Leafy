using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListCard : MonoBehaviour
{
    public TMP_Dropdown dropDown;
    public List<int> ids;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        CardList.OnScriptableObjectsLoaded += ListAll;
    }

    public void ListAll()
    {
        foreach (ScriptableCard sc in CardList.Cards)
        {
            dropDown.options.Add(new TMP_Dropdown.OptionData(sc.ID + "-" + sc.name, sc.artwork));
            ids.Add(sc.ID);
        }
        
    }

    public void Spawn()
    {
        CardUI c = GameManager.instance.SpawnCardRef(Vector3.zero, ids[dropDown.value]);
        c.transform.position = c.CameraCenterToPoint();
    }
}
