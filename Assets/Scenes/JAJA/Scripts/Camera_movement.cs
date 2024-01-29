using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_movement : MonoBehaviour
{
    public float Speed = 5f;
    public float zoomSpeed = 5f;
    public float minX = -10f;  // Limite minimale sur l'axe X
    public float maxX = 10f;   // Limite maximale sur l'axe X
    public float minY = -10f;  // Limite minimale sur l'axe Y
    public float maxY = 10f;
    public float minFOV = 5f;
    public float maxFOV = 20f;
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0f) * Speed * Time.deltaTime;

        Vector3 newPosition = transform.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        Camera cameraComponent = GetComponent<Camera>();
        cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize - scrollWheel * zoomSpeed, minFOV, maxFOV);
    }
}
