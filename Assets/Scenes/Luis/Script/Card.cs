using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Leafy.Manager;
using TMPro;

namespace Leafy.Objects
{
    [RequireComponent(typeof(CardBehavior))]
    public class Card : MonoBehaviour
    {
        public ScriptableCard info;
        public GameObject loader;
        
        private float movementSpeed = 30.0f;
        private float offsetY = 0.5f;
        private float offsetZ = 0.1f;
        
        private Collider2D collider;
        private TextMeshPro cardName;
        private SpriteRenderer artwork;
        private SpriteRenderer background;
        
        internal Card parent;
        internal Card child;
        internal int ID;


        private CardBehavior cardBehavior;
        private bool hovered;


        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            cardName = transform.GetChild(1).GetComponent<TextMeshPro>();
            artwork = transform.GetChild(2).GetComponent<SpriteRenderer>();
            background = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            UpdateCardInfo();
            if (TryGetComponent(out cardBehavior))
            {
                cardBehavior.Spawn();
            }
        }

        private void UpdateCardInfo()
        {
            cardName.text = info.name;
            background.color = info.background_color;
            artwork.sprite = info.artwork;
            ID = info.ID;
        }

        private void UpdateRenderID(int id)
        {
            background.sortingOrder = id;
            cardName.sortingOrder = id;
            artwork.sortingOrder = id;
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

            if (hovered)
            {
                cardBehavior.OnHoverStay();
            }
        }

        private void OnMouseEnter()
        {
            cardBehavior.OnHover();
            hovered = true;
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
                
            CardUtils.ApplyMethodOnStack(this, c => c.ChangeCollider(false));
            if(loader != null)
                Destroy(loader);
            
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
            parent = CardUtils.GetLastCard(card);
            if(parent != null)
                parent.child = this;
            
            cardBehavior.OnDrop();
            
            CardUtils.ApplyMethodOnStack(this, c => c.ChangeCollider(true));
        }

        /// <summary>
        /// Put the card to the front
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public void BringToFront()
        {
            Vector3 p = transform.position;
            p.z -= 9;
            transform.position = p;
        }

        /// <summary>
        /// Change all the stack collider to avoid collision and raycast
        /// </summary>
        /// <param name="enable"></param>
        public void ChangeCollider(bool enable)
        {
            collider.enabled = enable;
        }
        
        public void ChangeID(int id)
        {
            UpdateRenderID(id);
        }

        public void SetLoader(GameObject loader)
        {
            this.loader = loader;
        }

        /*public void ReduceLife()
        {
            life -= 1;
            if (life <= 0 && !infinite)
            {
                if(loader != null)
                    Destroy(loader);
                if (child != null)
                {
                    if (parent != null)
                    {
                        parent.child = child;
                        child.parent = parent;
                    }
                    else
                        child.transform.position = transform.position;
                }
                Destroy(gameObject);
            }
            else
            {
                GameManager.instance.TestCraft(this);
            }
        }*/
    }
}

