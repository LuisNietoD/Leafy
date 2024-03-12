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
    public float maxX = 20f;
    public float maxY = 20f;

    private CinemachineVirtualCamera mainCamera;

    private Vector3 lastMousePos;

    private Vector3 origin;
    private Vector3 difference;
    private bool drag;

    public ScaleWithFOV childScaler;

    private void Start()
    {
        mainCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        // Make sure this object's position matches the main camera's position
        if (transform.position != mainCamera.transform.position)
        {
            transform.position = mainCamera.transform.position;
        }

        // Movement using arrow keys or WASD
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0f) * Speed * Time.deltaTime;
        transform.Translate(movement);

        // Zoom using scroll wheel
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.m_Lens.OrthographicSize = Mathf.Clamp(mainCamera.m_Lens.OrthographicSize - scrollWheel * zoomSpeed, minFOV, maxFOV);
        
        if (Input.GetMouseButton(1))
        {
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (!drag)
            {
                drag = true;
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            transform.position = origin - difference;
        }

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, -maxX, maxX);
        p.y = Mathf.Clamp(p.y, -maxY, maxY);
        transform.position = p;
        childScaler.Scale();
    }
}
