using UnityEngine;
using Leafy.Objects;
using Leafy.Data;
using Leafy.Manager;

public class UiSpatialCard : CardBehavior
{
    private CardUI cardUI;
    private Card card;
    public GameObject spatialUIPrefab;
    private GameObject Ui;

    public UiSpatialCard(CardUI cardUI, GameObject UiPrefab)
    {
        this.cardUI = cardUI;
        card = this.cardUI.card;
        spatialUIPrefab = UiPrefab;
    }


    public override void Spawn()
    {
        if (spatialUIPrefab != null)
        { 
            Ui = GameManager.instance.SpawnObject(cardUI.transform.position,
                spatialUIPrefab);
        }
    }

    public override void OnDrag()
    {
    }

    public override void OnDrop()
    {
    }

    public override void OnClick()
    {
        Ui.SetActive(!Ui.activeSelf);
    }

    public override void OnHover()
    {
    }

    public override void OnHoverStay()
    {
    }

    public override void StayAction()
    {
        Ui.transform.position = cardUI.transform.position;
    }
}
