using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using UnityEngine;

namespace Leafy.Objects
{
    public class PressInterface : CardGameInterface
    {
        public Slot slot;
        

        public override void InterfaceAction()
        {
            if(slot.transform.childCount <= 0)
                return;
            
            Vector3 p = slot.transform.position;
            p.x += 2;
            GameManager.instance.SpawnCard(p, Craft.press[slot.transform.GetChild(0).GetComponent<CardUI>().ID]);
            Destroy(slot.transform.GetChild(0).gameObject);

        }
    }
}

