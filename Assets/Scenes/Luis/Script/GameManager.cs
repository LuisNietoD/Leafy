using System;
using Leafy.Objects;
using UnityEngine;
using Leafy.Data;

namespace Leafy.Manager
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager instance;
        
        public LayerMask cardLayer;
        
        private Card draggedCard;
        private Card hoveredCard;

        private int ID = 0;
        
        private float draggedCardSpeed = 10;
        private Vector3 offset;
        private float frontViewZ = -9;

        public GameObject crafterPrefab;
        public float crafterOffsetY;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }

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
                //add event for update craft loader
            }
            //Drop the dragged card
            else if (Input.GetMouseButtonUp(0) && draggedCard != null)
            {
                
                draggedCard.Drop(hoveredCard);
                Card firstStackCard = draggedCard.GetFirstCard(draggedCard);
                int craft = Craft.GetCraft(firstStackCard.GetStackIDList(firstStackCard));
                if (craft >= 0)
                {
                    LaunchCraft(craft, firstStackCard);
                }
                
                draggedCard = null;
            }
        }

        public void TestCraft(Card card)
        {
            Card firstStackCard = card.GetFirstCard(card);
            int craft = Craft.GetCraft(firstStackCard.GetStackIDList(firstStackCard));
            
            if (craft >= 0 && firstStackCard.loader == null)
            {
                LaunchCraft(craft, firstStackCard);
            }
        }

        private void LaunchCraft(int craftID, Card firstStackCard)
        {
            ScriptableCard toCraft = CardList.GetCardByID(craftID);
            if(toCraft == null)
                Debug.LogError("Cannot find this craft ID");

            Vector3 pos = firstStackCard.transform.position;
            pos.y += crafterOffsetY;
            GameObject crafter = Instantiate(crafterPrefab, pos, Quaternion.identity);

            CraftLoading cl = crafter.GetComponent<CraftLoading>();

            cl.timeToCraft = toCraft.timeToCraft;
            cl.drop = toCraft;
            cl.stack = firstStackCard.GetStackList(firstStackCard);

            foreach (Card c in cl.stack)
            {
                c.SetLoader(cl.gameObject);
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

