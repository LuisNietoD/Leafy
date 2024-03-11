using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 cursorHotSpot;
    
    // Start is called before the first frame update
    void Start()
    {
        cursorHotSpot = new Vector2(cursorTexture.width / 2, 10);
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.ForceSoftware);
    }
    
}
