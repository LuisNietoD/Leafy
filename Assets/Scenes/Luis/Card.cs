using UnityEngine;
using System;
using System.Collections;

namespace Leafy.Objects
{
    public class Card : MonoBehaviour
    {
        public float rotationForce = 0.1f;
        public float rotationSpeed = 5.0f;
        public float movementSpeed = 5.0f;
        private GameObject shadow;
        public float shadowOffsetY = 1;
        public float shadowOffsetX = 1;
        public float maxRotation;
        
        Vector3 lastMousePos;
        public Vector3 mouseDelta
        {
            get
            {
                return Input.mousePosition - lastMousePos;
            }
        }

        void Start()
        {
            lastMousePos = Input.mousePosition;
            shadow = transform.GetChild(0).gameObject;
        }
        
        
        private void OnMouseDrag()
        {
            drag = true;
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            
            Vector3 moveRotation = new Vector3(0, 0, 0);
            moveRotation.y = Mathf.Clamp(rotationForce * mouseDelta.x, -70f, 70f);
            moveRotation.x = Mathf.Clamp(rotationForce/2 * mouseDelta.y, -70f, 70f);
            moveRotation.z = 0;
            Quaternion targetRotation = Quaternion.Euler(moveRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                transform.eulerAngles.y, 0);

            float rawRotationY = transform.eulerAngles.y;
            if (rawRotationY > 180)
            {
                rawRotationY = -180 + (rawRotationY - 180);
            }
            
            float rawRotationX = transform.eulerAngles.x;
            if (rawRotationX > 180)
            {
                rawRotationX = -180 + (rawRotationX - 180);
            }
            shadow.transform.localPosition = new Vector3(-shadowOffsetX * (rawRotationY / maxRotation), 
                -shadowOffsetY * (rawRotationX / maxRotation), 1f);
            
            lastMousePos = Input.mousePosition; 
        }

        private bool drag;

        private void LateUpdate()
        {
            if (!Input.GetMouseButton(0))
            {
                drag = false;
            }
            
            if (!drag)
            {
                Vector3 moveRotation = new Vector3(0, 0, 0);
                Quaternion targetRotation = Quaternion.Euler(moveRotation);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, (rotationSpeed * 3) * Time.deltaTime);
            }
        }
    }
}

