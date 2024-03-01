using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Objects;
using UnityEngine;

namespace Leafy.Objects
{
    public class Lever : MonoBehaviour
    {
        public float actionTime = 0.5f;
        public bool moving;
        public float t = 0;
        public bool cooldown;
        private CardGameInterface cardInterface;

        private void Start()
        {
            cardInterface = transform.parent.GetComponent<CardGameInterface>();
        }

        private void Update()
        {
            if (!moving)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                if (hit.collider != null)
                {
                    if (hit.collider.transform == transform && Input.GetMouseButtonDown(0))
                    {
                        moving = true;
                    }
                }
            }

            if (moving)
            {
                t += Time.deltaTime;
                float y;
                if(t <= actionTime/2)
                    y = Mathf.Lerp(0.3f, -0.3f, t / (actionTime / 2));
                else
                    y = Mathf.Lerp(-0.3f, 0.3f, (t-actionTime/2) / (actionTime / 2));

                transform.localScale = new Vector3(0.3f, y, 0.3f);
                if (t > actionTime)
                {
                    moving = false;
                    transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    t = 0;
                    cooldown = false;
                }

                if (t > actionTime / 2 && !cooldown)
                {
                    cooldown = true;
                    cardInterface.InterfaceAction();
                }
            }
        }
    }
}
