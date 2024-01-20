using System;
using Leafy.Objects;
using UnityEngine;

namespace Leafy.Manager
{
    public class GameManager: MonoBehaviour
    {
        public LayerMask cardLayer;
        
        private Card draggedCard;
        private Card hoveredCard;

        private int ID = 0;
        
        private float draggedCardSpeed = 10;
        private Vector3 offset;
        private float frontViewZ = -9;
        
        private void Update()
        {
            hoveredCard = RayTestCard();

            //Grab the hovered card and drag it
            if (Input.GetMouseButtonDown(0) && hoveredCard != null)
            {
                offset = hoveredCard.SetDragged();
                draggedCard = hoveredCard;
                draggedCard.FrontViewAllCard(draggedCard.GetLastCard(draggedCard));
                draggedCard.ChangeID(ID++);
                hoveredCard = null;
            }
            //Drop the dragged card
            else if (Input.GetMouseButtonUp(0) && draggedCard != null)
            {
                draggedCard.Drop(hoveredCard);
                draggedCard = null;
            }
        }

        private void FixedUpdate()
        {
            //Drag the card with a little delay
            if (draggedCard != null)
            {
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                Vector2 position = Vector2.Lerp(draggedCard.transform.position, targetPosition, draggedCardSpeed * Time.deltaTime);
                float z = frontViewZ;
                draggedCard.transform.position = new Vector3(position.x, position.y, z);
            }
        }

        /// <summary>
        /// Return the first card hit on the board
        /// </summary>
        /// <returns></returns>
        public Card RayTestCard()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, cardLayer);

            if (hit.collider != null)
            {
                Card card = hit.collider.GetComponent<Card>();
                if (card != null)
                {
                    return card;
                }
            }
            return null;
        }
    }
}

