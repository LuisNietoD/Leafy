using UnityEngine;
using Leafy.Data;
using Leafy.Manager;
using TMPro;

namespace Leafy.Objects
{
    public class CardUI : MonoBehaviour
    {
        public Card card;
        public CardBehavior behavior;
        public GameObject loader;
        public GameObject pack;
        public int idStart;
        public int uniqueID;

        private float movementSpeed = 30.0f;
        private float offsetY = 0.5f;
        private float offsetZ = 0.1f;

        private Collider2D collider;
        private TextMeshPro cardName;
        private SpriteRenderer artwork;
        private SpriteRenderer background;

        internal CardUI parent;
        internal CardUI child;
        internal int ID;


        //private CardBehavior cardBehavior;
        private bool hovered;
        public bool dragged;


        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            cardName = transform.Find("Model/CARD NAME").GetComponent<TextMeshPro>();
            artwork = transform.Find("Model/ICON").GetComponent<SpriteRenderer>();
            background = transform.Find("Model/BACKGROUND").GetComponent<SpriteRenderer>();

        }

        private void Start()
        {
            CardList.OnScriptableObjectsLoaded += OnScriptableObjectsLoadedHandler;
            if (behavior != null)
                behavior.Spawn();
            uniqueID = CardUtils.ID++;
            /*if (TryGetComponent(out cardBehavior))
            {
                cardBehavior.Spawn();
            }*/
        }

        private void OnDestroy()
        {
            CardList.OnScriptableObjectsLoaded -= OnScriptableObjectsLoadedHandler;
        }

        void OnScriptableObjectsLoadedHandler()
        {
            //UpdateCardInfo(new Card(CardList.GetRandomCard()));
            UpdateCardInfo(new Card(CardList.GetCardByID(idStart)));
        }

        public void UpdateCardInfo(Card c)
        {
            card = c;
            cardName.text = card.name;
            background.color = card.backgroundColor;
            artwork.sprite = card.artwork;
            ID = card.ID;
            if (card.harvestable && behavior == null)
            {
                behavior = new Harvestable(this);
            }
            else if (behavior == null)
            {
                behavior = new ClassicCard();
            }
        }

        private void UpdateRenderID(int id)
        {
            background.sortingOrder = id;
            cardName.sortingOrder = id;
            artwork.sortingOrder = id;
            cardName.sortingOrder = id;
        }

        private void FixedUpdate()
        {
            //Move the card with a little delay
            if (parent != null)
            {
                Vector3 targetPosition = parent.transform.position;
                targetPosition.y -= offsetY;
                Vector2 position = Vector2.Lerp(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                float z = parent.transform.position.z - offsetZ;
                transform.position = new Vector3(position.x, position.y, z);
            }

            if (loader != null)
            {
                Vector3 pos = CardUtils.GetRootCard(this).transform.position;
                pos.y += 2;
                loader.transform.position = pos;
            }

            TestCollision();
            behavior?.StayAction();
        }

        public float pushForce = 1.0f;
        public LayerMask cardLayer;

        public void PushCard(Vector2 force)
        {
            transform.Translate(force);
        }

        private void TestCollision()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0f, cardLayer);

            foreach (var collider in colliders)
            {
                CardUI otherCard = collider.GetComponent<CardUI>();

                if (otherCard != null && otherCard != this && !dragged && !CardUtils.IsInTheSameStack(this, otherCard))
                {
                    if (otherCard.card.ID == card.ID && parent == null && otherCard.child == null && uniqueID > otherCard.uniqueID)
                    {
                        SetParent(otherCard);
                        CardUtils.ApplyMethodOnStack(this, c => c.ChangeID(GameManager.instance.ID++));
                        return;
                    }

                    Vector2 pushDirection = transform.position - collider.transform.position;
                    pushDirection.Normalize();
                    float forcePercent = Mathf.Clamp01(Vector2.Distance(otherCard.transform.position, transform.position));

                    CardUtils.GetRootCard(this).PushCard(pushDirection * (pushForce * forcePercent));
                }

                if (collider.CompareTag("Sell") && CardUtils.GetRootCard(this) == this)
                {
                    SellCard();
                }

                if (collider.CompareTag("Buy"))
                {
                    BuyCard();
                }
            }
        }
        private void SellCard()
        {
            if (card.sellable)
            {
                // Définir le nombre de nouveaux prefabs à créer


                // Crée deux nouveaux prefabs à la position actuelle
                for (int i = 0; i < card.price; i++)
                {
                    GameManager.instance.SpawnCard(new Vector3(-4, -3, 2), 1);
                }

                CardUtils.ApplyMethodOnStack(this, c => Destroy(c.gameObject));
            }
            else
            {
                Debug.Log("Tu peux pas gros bouff");
            }
        }

        private void BuyCard()
        {
            if (card.ID == 1)
            {
                GameObject newObject = Instantiate(pack, new Vector3(0, 0, 2), Quaternion.identity);
                CardUtils.ApplyMethodOnStack(this, c => Destroy(c.gameObject));
            }
        }



        private void OnMouseEnter()
        {
            //cardBehavior.OnHover();
            hovered = true;
        }


        /// <summary>
        /// Change all necessary value to make a card the main dragged object
        /// </summary>
        /// <returns></returns>
        public Vector3 SetDragged()
        {
            SetParent(null);

            CardUtils.ApplyMethodOnStack(this, c => c.ChangeCollider(false));

            CardUtils.ApplyMethodOnStack(this, c => c.dragged = true);

            return transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }


        /// <summary>
        /// Change all necessary value to drop the card
        /// </summary>
        /// <param name="cardUI"></param>
        public void Drop(CardUI cardUI)
        {
            Vector3 p = transform.position;
            p.z = 0;
            transform.position = p;
            SetParent(CardUtils.GetLastCard(cardUI));

            //cardBehavior.OnDrop();

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

        public void ReduceLife()
        {
            if (card.harvestable && !card.infinite)
            {
                card.life--;
                if (card.life <= 0)
                {
                    Destroy(loader);
                    if (child != null)
                        child.SetParent(parent);
                    Destroy(gameObject);
                }
            }
        }

        public void ChangeID(int id)
        {
            UpdateRenderID(id);
        }

        public void SetLoader(GameObject loader)
        {
            this.loader = loader;
        }

        public void SetParent(CardUI c)
        {
            if (parent != null)
            {
                parent.child = null;
                if (parent.loader != null)
                    Destroy(parent.loader);
            }

            if (c != null)
            {
                if ((card.harvestable && c.card.ID == card.ID) || !card.harvestable)
                    parent = c;
            }
            else
            {
                parent = null;
            }


            if (parent != null)
                c.child = this;
        }
    }
}