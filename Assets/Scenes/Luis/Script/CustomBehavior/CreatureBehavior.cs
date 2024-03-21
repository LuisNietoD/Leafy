using Leafy.Data;
using UnityEngine;

namespace Leafy.Objects
{
    public class CreatureBehavior: CardBehavior
    {
        public CardUI cardUI;
        private Card card;
        public float moveSpeed = 5f; // Adjust the speed of movement
        public float radius = 10f; // Radius within which the object can move

        private Vector3 targetPosition;
        
        public CreatureBehavior(CardUI c)
        {
            cardUI = c;
            card = cardUI.card;
        }
        
        public override void Spawn()
        {
            SetRandomTargetPosition();
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
        }

        public override void OnHoverStay()
        {
        }

        public override void StayAction()
        {
            cardUI.transform.position = Vector3.Lerp(cardUI.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(cardUI.transform.position, targetPosition) < 0.1f)
            {
                SetRandomTargetPosition();
            }
        }
        
        private void SetRandomTargetPosition()
        {
            // Generate a random point within the specified radius
            Vector2 randomPoint = Random.insideUnitCircle * radius;
            targetPosition = new Vector3(randomPoint.x, 0f, randomPoint.y);
        }
    }
}