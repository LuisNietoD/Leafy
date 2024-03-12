using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ButtonLink
{
    public GameObject button;
    public GameObject ui;
}

public class HUDButton : MonoBehaviour
{
    public List<ButtonLink> links = new List<ButtonLink>();

    public ButtonLink active = null;
    public LayerMask buttonLayer;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, buttonLayer);

            if (hit.collider != null)
            {
                Debug.Log("Test");
                ChangeActive(links.FirstOrDefault(l => l.button == hit.transform.gameObject));
            }
        }
    }

    public void ChangeActive(ButtonLink a)
    {
        if (active.ui != null)
        {
            SpriteRenderer s = active.button.GetComponent<SpriteRenderer>();
            s.color = new Color(0.8f, 0.8f, 0.8f);
            s.sortingOrder = -1;
            active.ui.SetActive(false);
        }

        if (a.ui != null)
        {
            SpriteRenderer s = a.button.GetComponent<SpriteRenderer>();
            s.color = Color.white;
            s.sortingOrder = 1;
            a.ui.SetActive(true);
            active = a;

            Vector3 p = transform.parent.localPosition;
            p.x = -10;
            transform.parent.localPosition = p;
        }
        else
        {
            Vector3 p = transform.parent.localPosition;
            p.x = -21.4f;
            transform.parent.localPosition = p;
        }
    }
}
