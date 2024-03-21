using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClicker : MonoBehaviour
{
    public LayerMask uiLayer;
    
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, uiLayer);

        if (hit.collider != null && Input.GetMouseButtonUp(0))
        {
            if (hit.transform.TryGetComponent(out SpriteButton button))
            {
                button.OnClick();
            }
        }
    }
}
