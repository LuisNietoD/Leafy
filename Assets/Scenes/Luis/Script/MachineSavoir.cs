using System.Collections.Generic;
using System.Linq;
using Leafy.Data;
using Leafy.Manager;
using UnityEngine;

namespace Leafy.Objects
{
    public class MachineSavoir: CardBehavior
    {
        private CardUI cardUI;
        private Card card;
        


        public MachineSavoir(CardUI cardUI)
        {
            this.cardUI = cardUI;
            card = this.cardUI.card;
        }
        
        public override void Spawn()
        {
            cardUI.inventory.SetActive(true);
            cardUI.slotIcon.sprite = CardList.GetCardByID(41).artwork;
        }

        public override void OnDrag()
        {
        }

        public override void OnDrop()
        {
        }

        public override void OnClick()
        {
            
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
            
            
            if(cardUI.child == null && cardUI.loader != null)
                GameManager.instance.DestroyObject(cardUI.loader);
            
            if (cardUI.child != null && cardUI.loader == null)
            {
                if (cardUI.child.ID == 41)
                {
                    card.actualSavoir++;
                    GameManager.instance.DestroyObject(cardUI.child.gameObject);
                }
                
                if (card.SavoirList != null)
                {
                    if (card.SavoirList[card.recipeIndex].pointDeSavoir <= card.actualSavoir)
                    {
                        card.actualSavoir -= card.SavoirList[card.recipeIndex].pointDeSavoir;
                        Vector3 p = cardUI.transform.position;
                        p.x += 3;
                        GameManager.instance.SpawnStackPrecise(p, card.SavoirList[card.recipeIndex].result);
                        card.recipeIndex++;
                    }
                    
                    if(card.SavoirList.Count > card.recipeIndex)
                        cardUI.slotText.text = card.actualSavoir + "/" + card.SavoirList[card.recipeIndex].pointDeSavoir;
                }
            }

        }
    }
}