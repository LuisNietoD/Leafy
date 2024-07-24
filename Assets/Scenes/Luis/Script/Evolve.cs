using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;

namespace Leafy.Objects
{
    public class Evolve: CardBehavior
    {
        private CardUI cardUI;
        private Card card;
        private float elapsed;
        
        public Evolve(CardUI cardUI)
        {
            this.cardUI = cardUI;
            card = this.cardUI.card;
        }
        
        public override void Spawn()
        {
            GameManager.instance.LaunchCraft(card.evolutions.PickValue().ID, new List<CardUI>(){cardUI}, 
                card.evolveTime, true);
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
            Debug.Log("test");
        }
    }
}