using System.Collections;
using System.Collections.Generic;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;

public class MixeurInterface : CardGameInterface
{
    public GameObject first;
    public GameObject second;

    public override void InterfaceAction()
    {
        if (first.transform.childCount > 0 && second.transform.childCount > 0)
        {
            List<int> cards = new List<int>();
            cards.Add(first.transform.GetChild(0).GetComponent<CardUI>().ID);
            cards.Add(second.transform.GetChild(0).GetComponent<CardUI>().ID);

            int result = Craft.GetMixCraft(cards);

            if (result != -1)
            {
                Vector3 p = transform.position;
                p.y -= 3.5f;
                GameManager.instance.SpawnCard(p, result);
                Destroy(first.transform.GetChild(0).gameObject);
                Destroy(second.transform.GetChild(0).gameObject);
            }
        }
    }
}
