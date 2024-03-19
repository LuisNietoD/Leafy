using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    
    public GameObject Btn;
    public GameObject Selection;
    public GameObject Selection2;
    public GameObject Delete;
    
    // Start is called before the first frame update
    void Start()
    {
        Btn.SetActive(true);
        Selection.SetActive(false);
        Selection2.SetActive(false);
        Delete.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Selection.activeSelf == true)
        {
            Btn.SetActive(false);
        }
        if (Selection2.activeSelf == true)
        {
            Selection.SetActive(false);
        }
        if (Delete.activeSelf == true)
        {
            Selection2.SetActive(false);
        }
        
       
        
        
    }
}
