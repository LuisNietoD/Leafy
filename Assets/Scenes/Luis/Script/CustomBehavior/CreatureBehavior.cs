using Leafy.Data;
using Leafy.Manager;
using UnityEngine;

namespace Leafy.Objects
{
    public class CreatureBehavior: CardBehavior
    {
        public CardUI cardUI;
        private Card card;
        public float moveSpeed = 3f; // Adjust the speed of movement
        public float radius = 5f; // Radius within which the object can move
        private bool canMove = true;
        private float elapsedTime;

        private Vector3 targetPosition;
        
        
        public CreatureBehavior(CardUI c)
        {
            cardUI = c;
            card = cardUI.card;
        }
        
        public override void Spawn()
        {
            if (!card.creatureChanged)
            {
                card.artwork = CreaturePicker.instance.GetCreature();
                cardUI.artwork.sprite = card.artwork;
                card.creatureChanged = true;
            }

            elapsedTime = 2;
            targetPosition = cardUI.transform.position;
        }

        public override void OnDrag()
        {
            canMove = false;
        }

        public override void OnDrop()
        {
            SetRandomTargetPosition();
            canMove = true;
        }

        public override void OnClick()
        {
            if (card.tool != 0)
            {
                Vector3 p = cardUI.transform.position;
                p.x += 3;
                GameManager.instance.SpawnCard(p, card.tool);
                card.tool = 0;
            }
            else
            {
                CreaturePicker.instance.OpenRename(cardUI);
            }
        }

        public override void OnHover()
        {
        }

        public override void OnHoverStay()
        {
        }

        public override void StayAction()
        {
            if (cardUI.child != null && card.tool == 0)
            {
                switch (cardUI.child.ID)
                {
                    case 109:
                        card.tool = 109;
                        GameManager.instance.DestroyObject(cardUI.child.gameObject);
                        break;
                    case 110:
                        card.tool = 110;
                        GameManager.instance.DestroyObject(cardUI.child.gameObject);
                        break;
                    case 112:
                        card.tool = 112;
                        GameManager.instance.DestroyObject(cardUI.child.gameObject);
                        break;
                }
            }
            
            switch (card.tool)
            {
                case 109:
                    Explorer();
                    break;
                case 110:
                    Miner();
                    break;
                case 112:
                    Translator();
                    break;
                case 0:
                    elapsedTime -= Time.deltaTime;
                    break;
            }

            if (cardUI.parent == null && cardUI.child == null)
            {
                if (canMove && cardUI.loader == null)
                {
                    cardUI.transform.position =
                        Vector3.Lerp(cardUI.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                }

                if (Vector3.Distance(cardUI.transform.position, targetPosition) < 0.1f && elapsedTime <= 0 && cardUI.loader == null)
                {
                    SetRandomTargetPosition();
                    elapsedTime = Random.Range(3f, 5f);
                    canMove = true;
                }
                
            }

        }

        public void Explorer()
        {
            if (cardUI.parent != null)
            {
                if (cardUI.parent.ID == 135)
                {
                    GameManager.instance.LaunchCraft(cardUI.parent.card.drop.PickValue().ID, CardUtils.GetStackCardList(cardUI.parent), cardUI.parent.card.harvestTime, false);
                }
            }
            else if (GameManager.instance.uniqueCard != 0)
            {
                if (elapsedTime - Time.deltaTime <= 0 && canMove)
                {
                    if (Random.Range(0, 10f) < 1.5f)
                    {
                        elapsedTime = 5;
                        canMove = false;
                        GameManager.instance.LaunchCraft(GameManager.instance.uniqueCard, CardUtils.GetStackCardList(cardUI), 4, false, true);
                    }
                }
            }

            elapsedTime -= Time.deltaTime;
        }
        
        public void Miner()
        {
            if (cardUI.parent != null)
            {
                if (cardUI.parent.ID == 134)
                {
                    GameManager.instance.LaunchCraft(cardUI.parent.card.drop.PickValue().ID, CardUtils.GetStackCardList(cardUI.parent), cardUI.parent.card.harvestTime, false);
                }
            }

            elapsedTime -= Time.deltaTime;
        }
        
        public void Translator()
        {
            if (cardUI.parent != null)
            {
                if (cardUI.parent.ID == 136)
                {
                    GameManager.instance.LaunchCraft(cardUI.parent.card.drop.PickValue().ID, CardUtils.GetStackCardList(cardUI.parent), cardUI.parent.card.harvestTime, false);
                }
            }
            
            elapsedTime -= Time.deltaTime;
        }
        
        private void SetRandomTargetPosition()
        {
            // Generate a random point within the specified radius
            Vector2 randomPoint = Random.insideUnitCircle * radius;
            Vector3 cardPosition = cardUI.transform.position;
            targetPosition = new Vector3(
                Mathf.Clamp(cardPosition.x + randomPoint.x, -GameManager.instance.maxX, GameManager.instance.maxX),
                Mathf.Clamp(cardPosition.y + randomPoint.y, -GameManager.instance.maxY, GameManager.instance.maxY),
                cardPosition.z);
        }
    }
}