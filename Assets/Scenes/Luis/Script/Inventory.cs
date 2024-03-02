using System.Collections.Generic;
using System.Linq;
using Leafy.Data;
using Leafy.Manager;
using UnityEngine;

namespace Leafy.Objects
{
    public class Inventory: CardBehavior
    {
        private CardUI cardUI;
        private Card card;
        private int actualstoredCardID;
        private int actualstoredCard;

        public Inventory(CardUI cardUI)
        {
            this.cardUI = cardUI;
            card = this.cardUI.card;
        }
        
        public override void Spawn()
        {
            cardUI.inventory.SetActive(true);
            card.stockEmpty = true;
            
            if (!card.preciseCard)
                cardUI.slotIcon.enabled = false;
            else
                cardUI.slotIcon.sprite = CardList.GetCardByID(card.preciseCardID).artwork;
        }

        public override void OnDrag()
        {
        }

        public override void OnDrop()
        {
        }

        public override void OnClick()
        {
            Vector3 p = cardUI.transform.position;
            p.x += 3;
            if (actualstoredCard >= 5)
            {
                GameManager.instance.SpawnStack(p, actualstoredCardID, 5);
                actualstoredCard -= 5;
            }
            else if (actualstoredCard > 0)
            {
                GameManager.instance.SpawnStack(p, actualstoredCardID, actualstoredCard);
                actualstoredCard = 0;
            }

            if (actualstoredCard <= 0)
            {
                actualstoredCardID = 0;
                if (!card.preciseCard)
                    cardUI.slotIcon.enabled = false;
                card.stockEmpty = true;
            }
            else
            {
                card.stockEmpty = false;
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
            if (cardUI.child != null)
            {
                if (card.preciseCard && card.preciseCardID == cardUI.child.ID)
                {
                    actualstoredCardID = card.preciseCardID;
                    actualstoredCard++;
                    GameManager.instance.DestroyObject(cardUI.child.gameObject);
                }
                else if (!card.preciseCard && (cardUI.child.ID == actualstoredCardID || actualstoredCardID == 0) && cardUI.child.card.type == card.typeSlot.ToString())
                {
                    actualstoredCardID = cardUI.child.ID;
                    actualstoredCard++;
                    GameManager.instance.DestroyObject(cardUI.child.gameObject);
                }

                if (cardUI.child.ID == cardUI.ID)
                {
                    if (cardUI.child.card.stockEmpty)
                    {
                        card.slotAmount += cardUI.child.card.slotAmount;
                        GameManager.instance.DestroyObject(cardUI.child.gameObject);
                    }
                }
            }

            if (actualstoredCardID != 0)
            {
                cardUI.slotIcon.enabled = true;
                cardUI.slotIcon.sprite = CardList.GetCardByID(actualstoredCardID).artwork;
            }
            else if (card.preciseCard)
            {
                cardUI.slotIcon.sprite = CardList.GetCardByID(card.preciseCardID).artwork;
            }

            cardUI.slotText.text = actualstoredCard + "/" + card.slotAmount;
        }
    }
}