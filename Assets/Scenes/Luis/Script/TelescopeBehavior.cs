using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using UnityEngine;

namespace Leafy.Objects
{
    public class TelescopeBehavior: CardBehavior
    {
        private CardUI cardUI;
        private Card card;
        private bool interfaceActive = false;
        
        public TelescopeBehavior(CardUI cardUI)
        {
            this.cardUI = cardUI;
            card = this.cardUI.card;
        }
        
        public override void Spawn()
        {
            cardUI.interfaceSlot.transform.GetChild(0).gameObject.SetActive(interfaceActive);
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
            cardUI.interfaceSlot.transform.GetChild(0).gameObject.SetActive(interfaceActive);
            
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
            if(cardUI.interfaceSlot.transform.GetChild(0).childCount <= 4)
                card.elapsedSpaceTime += Time.deltaTime;

            if (SpaceObjectList.instance.actual == -1 && card.elapsedSpaceTime >= card.spaceSpawnTime)
            {
                card.spaceIndex++;
                SpaceObjectList.instance.actual = card.spaceIndex;
                card.elapsedSpaceTime = 0;
            }
            if (card.elapsedSpaceTime >= card.spaceSpawnTime && SpaceObjectList.instance.spaceObject.Count > card.spaceIndex 
                                                             && cardUI.interfaceSlot.transform.GetChild(0).childCount <= 4)
            {
                cardUI.interfaceSlot.transform.GetChild(1).gameObject.SetActive(true);
                GameObject g = GameManager.instance.SpawnObject(Vector3.zero, SpaceObjectList.instance.spaceObject[card.spaceIndex]);
                g.transform.parent = cardUI.interfaceSlot.transform.GetChild(0);
                g.transform.position = cardUI.interfaceSlot.transform.GetChild(0).transform.GetChild(3).position;
                SpaceObjectList.instance.actual = card.spaceIndex;
            }

            if (cardUI.interfaceSlot.transform.GetChild(0).childCount > 4)
            {
                cardUI.interfaceSlot.transform.GetChild(1).gameObject.SetActive(!interfaceActive);
            }
            else
            {
                cardUI.interfaceSlot.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}