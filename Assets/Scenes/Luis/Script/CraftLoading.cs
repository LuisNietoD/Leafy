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
            GameObject d = Instantiate(cardPrefab, stack[0].transform.position, Quaternion.identity);
            d.GetComponent<CardUI>().card = new Card(drop);

            foreach (CardUI card in stack)
            {
                Destroy(card.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
