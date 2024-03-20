using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSequence : MonoBehaviour
{
    public RawImage img;
    public Material mat;
    private bool end;
    private float endTime = 10;
    private float elapsedTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        mat = new Material(img.material);
        img.material = mat;
    }

    public void End()
    {
        end = true;
        mat.SetFloat("_ShakeUvSpeed", 18f);
    }

    private void FixedUpdate()
    {
        
        if (end)
        {
            elapsedTime += Time.deltaTime;
            
            mat.SetFloat("_FishEyeUvAmount", Mathf.Lerp(0, 0.18f, Mathf.Clamp(elapsedTime, 0, 3)/3));
            mat.SetFloat("_DistortAmount", Mathf.Lerp(0, 0.4f, Mathf.Clamp(elapsedTime, 0, 6)/6));
            mat.SetFloat("_RoundWaveStrength", Mathf.Lerp(0, 0.6f, Mathf.Clamp(elapsedTime, 0, 6)/6));
            mat.SetFloat("_OverlayBlend", Mathf.Lerp(0, 1f, Mathf.Clamp(elapsedTime-6, 0, 4)/4));
            mat.SetFloat("_GradBlend", Mathf.Lerp(0, 1f, Mathf.Clamp(elapsedTime-6, 0, 3)/3));
            mat.SetFloat("_HitEffectBlend", Mathf.Lerp(0, 1f, Mathf.Clamp(elapsedTime-9, 0, 1)));
            if (elapsedTime >= 9)
            {
                mat.SetFloat("_GradBlend", Mathf.Lerp(1, 0f, Mathf.Clamp(elapsedTime-9, 0, 1)));
            }
        }
    }
    
}
