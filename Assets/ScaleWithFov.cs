using Cinemachine;
using UnityEngine;

public class ScaleWithFOV : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;
    public float max;
    public float min;

    public void Scale()
    {
        if (mainCamera != null)
        {
            float orthoSize = mainCamera.m_Lens.OrthographicSize;
            float l = (orthoSize - 2) / 23;
            float scale = Mathf.Lerp(min, max, l);

            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            Debug.LogWarning("Main camera not assigned!");
        }
    }
}
