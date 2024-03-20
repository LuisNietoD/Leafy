using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    public List<GameObject> listCraft = new List<GameObject>();
    public int nextRecipeToUnlock;
    
    public void EnableNextRecipe()
    {
        if (nextRecipeToUnlock < listCraft.Count)
        {
            listCraft[nextRecipeToUnlock].SetActive(true);
            nextRecipeToUnlock++;
        }
    }
}
