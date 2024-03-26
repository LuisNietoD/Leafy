using System;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using RNGNeeds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Leafy.Objects
{
    public class Harvestable: CardBehavior
    {
        private CardUI cardUI;
        private Card card;
        private float shakeThreshold = 5f;
        private float elapsed;
        private Vector3 lastPosition;
        private float speed;
        private float elapsedTime;
        
        public float shakeIntensity = 0.2f;
        public float shakeSpeed = 10f;
        public int i = 0;

        public Harvestable(CardUI cardUI)
        {
            this.cardUI = cardUI;
            card = this.cardUI.card;
        }
        
        public override void Spawn()
        {
            lastPosition = cardUI.transform.position;
        }

        public override void OnDrag()
        {
        }

        public override void OnDrop()
        {
        }

        public override void OnClick()
        {
            cardUI.feedback_FullPlant.StopFeedbacks();
            cardUI.feedback_FullPlant.RestoreInitialValues();
            cardUI.feedback_FullPlant.ResetFeedbacks();
            cardUI.feedback_FullPlant.CanPlay = true;
            
            if (card.shakable)
            {
                if (card.precise)
                {
                    Vector3 pos = cardUI.transform.position; 
                    pos.x += 3;
                    pos.y -= 1.5f * i;
                    
                    GameManager.instance.SpawnCard(pos, card.preciseList[i].ID, cardUI);
                    cardUI.ReduceLife();
                    i++;
                }
                else
                {
                    Vector3 pos = cardUI.transform.position;
                    pos.x += 3;
                    pos.y -= 1.5f * i;
                    GameManager.instance.SpawnCard(pos, card.drop.PickValue().ID, cardUI);
                    QuestManager.instance.UpdateQuest(0, 1, 0);
                    cardUI.ReduceLife();
                    i++;
                }
                
            }
            
            Vector3 p = cardUI.transform.position;
            p.x += 4;
            if(card.cardIDs.Count > 0)
                GameManager.instance.SpawnStackPrecise(p, card.cardIDs);
            card.cardIDs.Clear();
        }

        public override void OnHover()
        {
        }

        public override void OnHoverStay()
        {
        }
        
        public override void StayAction()
        {
            if (cardUI.child != null && cardUI.loader == null)
            {
                if (card.activators.Contains(cardUI.child.ID))
                {
                    GameManager.instance.LaunchCraft(card.drop.PickValue().ID, 
                        new List<CardUI>() {cardUI, cardUI.child}, card.harvestTime);
                }
            }

            if (card.activators.Count <= 0 && !card.shakable)
            {
                if (card.storeCard && card.cardIDs.Count < card.inventorySize + (card.inventorySize * card.storageLevel))
                {
                    elapsedTime += Time.deltaTime; // Accumulate time using real-time seconds
                    float f = card.harvestTime - 0.3f;

                    if (elapsedTime >= f)
                    {
                        // Saturate elapsedTime to a maximum of card.plantRate to ensure it doesn't exceed the boundary
                        float adjustedElapsedTime = Mathf.Min(elapsedTime, card.harvestTime);
                        float blendValue = (adjustedElapsedTime - f) / 0.3f; // Calculate blend value within the last 0.2 seconds
                        cardUI.artwork.material.SetFloat("_HitEffectBlend", blendValue);
                    }
                    if (elapsedTime >= card.harvestTime - ((card.harvestTime / 10) * card.rateLevel))
                    {
                        card.cardIDs.Add(card.drop.PickValue().ID);
                        float r = Random.Range(0f, 1f);
                        Debug.Log(r);
                        if (r < 0.2f * card.productivityLevel)
                            card.cardIDs.Add(card.drop.PickValue().ID);
                        elapsedTime = 0;
                        cardUI.artwork.material.SetFloat("_HitEffectBlend", 0);
                    }
                }
                else if (!card.storeCard)
                {
                    GameManager.instance.LaunchCraft(card.drop.PickValue().ID, 
                        new List<CardUI>() {cardUI}, card.harvestTime);
                }
            }

            if (card.storeCard && card.cardIDs.Count == card.inventorySize + (card.inventorySize * card.storageLevel))
            {
                /*float offsetX = Mathf.PerlinNoise(0, Time.time * shakeSpeed) * shakeIntensity - shakeIntensity / 2f;
                float offsetY = Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * shakeIntensity - shakeIntensity / 2f;

                Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f);
                shakeOffset.y = cardUI.model.transform.localPosition.y;
                cardUI.model.transform.localPosition = Vector3.zero + shakeOffset;*/
                if(cardUI.feedback_FullPlant.CanPlay)
                    cardUI.feedback_FullPlant.PlayFeedbacks();
                cardUI.feedback_FullPlant.CanPlay = false;
                card.plantIsFull = true;
                cardUI.artwork.material.SetFloat("_HitEffectBlend", 0);
            }
            else
            {
                cardUI.feedback_FullPlant.StopFeedbacks();
                cardUI.feedback_FullPlant.CanPlay = true;
                card.plantIsFull = false;
            }


            elapsed += Time.deltaTime;
            Vector3 currentPosition = cardUI.transform.position;
            currentPosition.z = lastPosition.z;
            speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;
            lastPosition = currentPosition;
        }
        
    }
}