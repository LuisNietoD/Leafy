using Leafy.Data;
using UnityEngine;

namespace Leafy.Objects
{
    public class WateringCanBehavior: CardBehavior
    {
        public CardUI cardUI;
        private Card card;
        private bool interfaceActive = true;
        public GameObject i;

        public WateringCanBehavior(CardUI c)
        {
            cardUI = c;
            card = cardUI.card;
        }
        
        public override void Spawn()
        {
            Debug.Log("WATERINNNNNNG");
            card = cardUI.card;
            i = cardUI.transform.Find("INTERFACE").gameObject;
            i.SetActive(false);
        }

        public override void OnDrag()
        {
            i.SetActive(true);
        }

        public override void OnDrop()
        {
            i.SetActive(false);
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
            
        }
    }
}