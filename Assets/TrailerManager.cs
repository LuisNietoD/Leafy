using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerManager : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 4;
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            Time.timeScale = 1;
        }
    }
}
