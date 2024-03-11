using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using RNGNeeds;
using UnityEngine;

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
        private List<int> cardIDs = new List<int>(); 
        
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
                    cardUI.ReduceLife();
                    i++;
                }
                
            }
            
            Vector3 p = cardUI.transform.position;
            p.x += 4;
            if(cardIDs.Count > 0)
                GameManager.instance.SpawnStackPrecise(p, cardIDs);
            cardIDs.Clear();
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

            if (card.activators.Count <= 0 && !card.shakable && cardIDs.Count < card.inventorySize + (card.inventorySize * card.storageLevel))
            {
                elapsedTime += Time.deltaTime * card.plantRate;
                if (elapsedTime >= card.harvestTime - (card.harvestTime/10 * card.rateLevel))
                {
                    cardIDs.Add(card.drop.PickValue().ID);
                    if(Random.Range(0f, 1f) < 0.1f * card.productivityLevel)
                        cardIDs.Add(card.drop.PickValue().ID);
                    elapsedTime = 0;
                }
            }

            if (card.storeCard && cardIDs.Count == card.inventorySize + (card.inventorySize * card.storageLevel))
            {
                float offsetX = Mathf.PerlinNoise(0, Time.time * shakeSpeed) * shakeIntensity - shakeIntensity / 2f;
                float offsetY = Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * shakeIntensity - shakeIntensity / 2f;

                Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f);
                shakeOffset.y = cardUI.model.transform.localPosition.y;
                cardUI.model.transform.localPosition = Vector3.zero + shakeOffset;
                card.plantIsFull = true;
            }
            else
            {
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