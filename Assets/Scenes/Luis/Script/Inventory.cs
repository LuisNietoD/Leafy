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
            if (card.actualstoredCard >= 5)
            {
                GameManager.instance.SpawnStack(p, card.actualstoredCardID, 5);
                card.actualstoredCard -= 5;
            }
            else if (card.actualstoredCard > 0)
            {
                GameManager.instance.SpawnStack(p, card.actualstoredCardID, card.actualstoredCard);
                card.actualstoredCard = 0;
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
                    if (card.actualstoredCard < cardUI.card.slotAmount)
                    {
                        card.actualstoredCardID = card.preciseCardID;
                        card.actualstoredCard++;
                        GameManager.instance.DestroyObject(cardUI.child.gameObject);
                    }
                }
                else if (!card.preciseCard && (cardUI.child.ID == card.actualstoredCardID || card.actualstoredCardID == 0) && cardUI.child.card.type == card.typeSlot.ToString())
                {
                    if (card.actualstoredCard < cardUI.card.slotAmount)
                    {
                        card.actualstoredCardID = cardUI.child.ID;
                        card.actualstoredCard++;
                        GameManager.instance.DestroyObject(cardUI.child.gameObject);
                    }
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

            if (card.actualstoredCardID != 0)
            {
                cardUI.slotIcon.enabled = true;
                cardUI.slotIcon.sprite = CardList.GetCardByID(card.actualstoredCardID).artwork;
            }
            else if (card.preciseCard)
            {
                cardUI.slotIcon.sprite = CardList.GetCardByID(card.preciseCardID).artwork;
            }
            
            if (card.actualstoredCard <= 0)
            {
                card.actualstoredCardID = 0;
                if (!card.preciseCard)
                    cardUI.slotIcon.enabled = false;
                card.stockEmpty = true;
            }
            else if(card.actualstoredCard > 0)
            {
                card.stockEmpty = false;
            }

            cardUI.slotText.text = card.actualstoredCard + "/" + card.slotAmount;
        }
    }
}