using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPack : MonoBehaviour
{

    public GameObject Pack;
 
    // Start is called before the first frame update
    private void Start()
    {
        // Désactive le panneau au départ
        if (Pack != null)
        {
            Pack.SetActive(false);
        }
    }

    public void PackAccepted() 
    {
        if (Pack != null)
        {
            Pack.SetActive(true);
        }
    }

    public void EndOfPack() 
    {

    }

}
