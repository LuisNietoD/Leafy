using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using UnityEngine;

namespace Leafy.Objects
{
    public class ActivableInterface: CardBehavior
    {
        private CardUI cardUI;
        private Card card;
        private bool interfaceActive = true;
        private bool allPlantFull = false;
        public float shakeIntensity = 0.2f;
        public float shakeSpeed = 10f;
        
        public ActivableInterface(CardUI cardUI)
        {
            this.cardUI = cardUI;
            card = this.cardUI.card;
        }
        
        public override void Spawn()
        {
            
        }

        public override void OnDrag()
        {
            Debug.Log("OnDrag");
        }

        public override void OnDrop()
        {
            Debug.Log("OnDrop");
        }

        public override void OnClick()
        {
            interfaceActive = !interfaceActive;
            foreach (Transform child in cardUI.interfaceSlot.transform)
            {
                child.GetComponent<SpriteRenderer>().enabled = interfaceActive;
                if (child.childCount > 0)
                {
                    child.GetChild(0).GetComponent<CardUI>().model.SetActive(interfaceActive);
                    child.GetChild(0).GetChild(1).gameObject.SetActive(interfaceActive);
                }
            }
        }

        public override void OnHover()
        {
            Debug.Log("OnHover");
        }

        public override void OnHoverStay()
        {
            Debug.Log("OnHoverStay");
        }
        
        public override void StayAction()
        {
            allPlantFull = false;
            
            foreach (Transform child in cardUI.interfaceSlot.transform)
            {
                if (child.transform.childCount > 0)
                {
                    allPlantFull = true;
                }
            }
            
            foreach (Transform child in cardUI.interfaceSlot.transform)
            {
                if (child.TryGetComponent(out Slot s))
                {
                    if (!s.TestPlant())
                        allPlantFull = false;
                }
            }


            if (allPlantFull)
            {
                float offsetX = Mathf.PerlinNoise(0, Time.time * shakeSpeed) * shakeIntensity - shakeIntensity / 2f;
                float offsetY = Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * shakeIntensity - shakeIntensity / 2f;

                Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f);
                shakeOffset.y = cardUI.model.transform.localPosition.y;
                cardUI.model.transform.localPosition = Vector3.zero + shakeOffset;
            }
        }
        
    }
}