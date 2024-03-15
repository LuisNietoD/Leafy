using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    public Transform spawnPoint;

    public List<GameObject> objects;
    public float timeBetweenSpawn = 5;
    public GameObject actualObject;
    private float elapsedTime;
    
    void Update()
    {
        if(actualObject == null)
            elapsedTime += Time.deltaTime;
        
        if (elapsedTime >= timeBetweenSpawn && objects.Count > 0)
        {
            GameObject o = Instantiate(objects[0], spawnPoint.position, Quaternion.identity);
            objects.RemoveAt(0);
            actualObject = o;
            elapsedTime = 0;
        }
    }
}
