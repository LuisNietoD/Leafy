using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
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
            Debug.Log("OnDrag");
        }

        public override void OnDrop()
        {
            Debug.Log("OnDrop");
        }

        public override void OnClick()
        {
            Debug.Log("OnClick");
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
            
            if (cardUI.child != null && cardUI.loader == null)
            {
                if (card.activators.Contains(cardUI.child.ID))
                {
                    GameManager.instance.LaunchCraft(card.drop.PickValue().ID, new List<CardUI>() {cardUI, cardUI.child});
                }
            }
            
            elapsed += Time.deltaTime;
            Vector3 currentPosition = cardUI.transform.position;
            currentPosition.z = lastPosition.z;
            speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;
            lastPosition = currentPosition;

            if (card.shakable && speed > shakeThreshold && elapsed >= 0.8f)
            {
                elapsed = 0;
                Vector3 pos = cardUI.transform.position;
                pos.y -= 2;
                GameManager.instance.SpawnCard(pos, card.drop.PickValue().ID, cardUI);
                cardUI.ReduceLife();
            }
        }
        
    }
}