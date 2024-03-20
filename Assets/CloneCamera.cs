using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneCamera : MonoBehaviour
{
    public Camera cam;
    public Camera main;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize = main.orthographicSize;
        cam.transform.position = main.transform.position;
    }
}
