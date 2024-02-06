using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Camera_movement : MonoBehaviour
{
    public float Speed = 5f;
    public float zoomSpeed = 5f;
    public float minFOV = 5f;
    public float maxFOV = 20f;

    private Vector3 lastMousePos;

    void Update()
    {
        // Make sure this object's position matches the main camera's position
        if (transform.position != Camera.main.transform.position)
        {
            transform.position = Camera.main.transform.position;
        }
        
        // Movement using arrow keys or WASD
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0f) * Speed * Time.deltaTime;
        transform.Translate(movement);

        // Zoom using scroll wheel
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        CinemachineVirtualCamera cameraComponent = GetComponent<CinemachineVirtualCamera>();
        cameraComponent.m_Lens.OrthographicSize = Mathf.Clamp(cameraComponent.m_Lens.OrthographicSize - scrollWheel * zoomSpeed, minFOV, maxFOV);

        // Camera dragging with right mouse button
        if (Input.GetMouseButtonDown(1)) // Right mouse button clicked
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1)) // Right mouse button held down
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.Translate(-delta.x * Time.deltaTime, -delta.y * Time.deltaTime, 0);
            lastMousePos = Input.mousePosition;
        }
        
    }
}
