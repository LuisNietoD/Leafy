using UnityEngine;
using System;
using System.Collections;
using Leafy.Manager;
using Unity.VisualScripting;

namespace Leafy.Objects
{
    public class Card : MonoBehaviour
    {
        private float movementSpeed = 30.0f;
        private float offsetY = 0.5f;
        private float offsetZ = 0.1f;
        
        private Collider2D collider;
        
        private Card parent;
        private Card child;


        private void Awake()
        {
            collider = GetComponent<Collider2D>();
        }
        
        private void Update()
        {
            //Move the card with a little delay
            if(parent != null)
            {
                Vector3 targetPosition = parent.transform.position;
                targetPosition.y -= offsetY;
                Vector2 position = Vector2.Lerp(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                float z = parent.transform.position.z - offsetZ;
                transform.position = new Vector3(position.x, position.y, z);
            }
        }

        /// <summary>
        /// Change all necessary value to make a card the main dragged object
        /// </summary>
        /// <returns></returns>
        public Vector3 SetDragged()
        {
            if(parent != null)
                parent.child = null;
            parent = null;
                
            ChangeStackCollider(false);
            
            return transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        /// <summary>
        /// Change all necessary value to drop the card
        /// </summary>
        /// <param name="card"></param>
        public void Drop(Card card)
        {
            Vector3 p = transform.position;
            p.z = 0;
            transform.position = p;
            parent = GetLastCard(card);
            if(parent != null)
                parent.child = this;
            ChangeStackCollider(true);
        }

        
        /// <summary>
        /// Return the last card of the stack
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Card GetLastCard(Card card)
        {
            if (card == null)
                return null;
            if (card.child == null)
                return card;
            return GetLastCard(card.child);
        }

        /// <summary>
        /// Put all the stack on front view when dragged
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Card FrontViewAllCard(Card card)
        {
            Vector3 p = transform.position;
            p.z -= 9;
            transform.position = p;
            if (card.parent == null)
                return card;
            return FrontViewAllCard(card.parent);
        }

        /// <summary>
        /// Change all the stack collider to avoid collision and raycast
        /// </summary>
        /// <param name="enable"></param>
        public void ChangeStackCollider(bool enable)
        {
            if(child != null)
                child.ChangeStackCollider(enable);
            
            collider.enabled = enable;
        }
    }
}

