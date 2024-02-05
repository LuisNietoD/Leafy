using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Objects;
using UnityEngine;

public class CraftLoading : MonoBehaviour
{
    public List<CardUI> stack = new List<CardUI>();
    public GameObject cardPrefab;
    public ScriptableCard drop;
    public float timeToCraft = 1;
    public float elapsed = 0;
    public SpriteRenderer loadImage;

    private void Start()
    {
        loadImage.material = new Material(loadImage.material);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        loadImage.material.SetFloat("_RadialClip", Mathf.Lerp(360, 0, Mathf.Clamp01(elapsed/timeToCraft)));
        

        if (elapsed >= timeToCraft)
        {
            Vector3 pos = stack[0].transform.position;
            pos.x += 2;
            GameObject d = Instantiate(cardPrefab, pos, Quaternion.identity);
            if (d.TryGetComponent(out CardUI cardUI))
            {
                cardUI.UpdateCardInfo(new Card(drop));
            }
            

            foreach (CardUI card in stack)
            {
                card.ReduceLife();
                //Destroy(card.gameObject);
            }

            elapsed = 0;
            
        }
    }
}
