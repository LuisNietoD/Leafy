using System;
using UnityEngine;
using Leafy.Data;
using Leafy.Manager;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace Leafy.Objects
{
    public class CardUI : MonoBehaviour
    {
        public Card card;
        public CardBehavior behavior;
        public GameObject loader;
        public GameObject pack;
        public int uniqueID;

        private float movementSpeed = 30.0f;
        private float offsetY = 0.6f;
        private float offsetZ = 0.1f;

        private Collider2D collider;
        private TextMeshPro cardName;
        private SpriteRenderer artwork;
        private SpriteRenderer background;
        private TextMeshPro typeName;
        private SpriteRenderer bord;
        private SpriteRenderer shadow;
        private SpriteRenderer noEnergyIcon;
        private SpriteRenderer noEnergyBar;
        private GameObject noEnergy;
        private GameObject energy;
        private TextMeshPro energyText;
        private SpriteRenderer energyIcon;
        public GameObject inventory;
        public TextMeshPro slotText;
        public SpriteRenderer slotIcon;
        public GameObject model;
        public GameObject sellable;
        public TextMeshPro price;
        public SpriteRenderer priceIcon;
        public SpriteRenderer speedIcon;
        public SpriteRenderer growIcon;
        public SpriteRenderer stockIcon;

        public GameObject interfaceSlot;

        internal CardUI parent;
        internal CardUI child;
        internal int ID;

        private GameObject _interface;


        //private CardBehavior cardBehavior;
        private bool hovered;
        public bool dragged;

        private static int BuyPack = 0;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            cardName = transform.Find("MODEL/CARD NAME").GetComponent<TextMeshPro>();
            typeName = transform.Find("MODEL/TYPE").GetComponent<TextMeshPro>();
            artwork = transform.Find("MODEL/ICON").GetComponent<SpriteRenderer>();
            background = transform.Find("MODEL/BACKGROUND").GetComponent<SpriteRenderer>();
            bord = transform.Find("MODEL/BORD").GetComponent<SpriteRenderer>();
            shadow = transform.Find("SHADOW").GetComponent<SpriteRenderer>();
            energyIcon = transform.Find("MODEL/ENERGY/ICON").GetComponent<SpriteRenderer>();
            noEnergyIcon = transform.Find("MODEL/NOENERGY/ICON").GetComponent<SpriteRenderer>();
            noEnergyBar = transform.Find("MODEL/NOENERGY/BAR").GetComponent<SpriteRenderer>();
            noEnergy = transform.Find("MODEL/NOENERGY").gameObject;
            energy = transform.Find("MODEL/ENERGY").gameObject;
            energyText = transform.Find("MODEL/ENERGY/ENERGYTEXT").GetComponent<TextMeshPro>();
            model = transform.Find("MODEL").gameObject;
            slotIcon = transform.Find("MODEL/INVENTORY/ICON").GetComponent<SpriteRenderer>();
            slotText = transform.Find("MODEL/INVENTORY/SLOTTEXT").GetComponent<TextMeshPro>();
            inventory = transform.Find("MODEL/INVENTORY").gameObject;
            sellable = transform.Find("MODEL/SELLABLE").gameObject;
            priceIcon = transform.Find("MODEL/SELLABLE/ICON").GetComponent<SpriteRenderer>();
            price = transform.Find("MODEL/SELLABLE/PRICE").GetComponent<TextMeshPro>();
            speedIcon = transform.Find("MODEL/SPEEDICON").GetComponent<SpriteRenderer>();
            growIcon = transform.Find("MODEL/GROWICON").GetComponent<SpriteRenderer>();
            stockIcon = transform.Find("MODEL/STOCKICON").GetComponent<SpriteRenderer>();

            
            energy.SetActive(false);
            noEnergy.SetActive(false);
            inventory.SetActive(false);
            sellable.SetActive(false);
            speedIcon.enabled = false;
            growIcon.enabled = false;
            stockIcon.enabled = false;
            
        }

        private void Start()
        {
            behavior?.Spawn();
            uniqueID = CardUtils.ID++;
        }

        public void UpdateCardInfo(Card c)
        {
            card = c;
            cardName.text = card.name;
            typeName.text = card.type;
            background.color = card.backgroundColor;
            artwork.sprite = card.artwork;
            ID = card.ID;
            if (card.harvestable && behavior == null)
            {
                behavior = new Harvestable(this);
            }
            else if (card.evolve && behavior == null)
            {
                behavior = new Evolve(this);
            }
            else if (card.transmute && behavior == null)
            {
                behavior = new Transmute(this);
            }
            else if (card.activableInterface && behavior == null)
            {
                behavior = new ActivableInterface(this);
            }
            else if (card.inventory && behavior == null)
            {
                behavior = new Inventory(this);
            }
            else if (card.machineSavoir && behavior == null)
            {
                behavior = new MachineSavoir(this);
            }
            else if (card.customBehavior && behavior == null)
            {
                if(card.behavior == "WateringCanBehavior")
                    behavior = new WateringCanBehavior(this);
            }
            else if (behavior == null)
            {
                behavior = new ClassicCard();
            }
            

            foreach (GameObject i in card.interfaceList)
            {
                GameObject g = Instantiate(i, transform.position, Quaternion.identity);
                g.transform.SetParent(transform);
                g.transform.localPosition = Vector3.zero;
                g.name = "INTERFACE";

                interfaceSlot = g;
            }

            if (card.requiereEnergy)
            {
                energy.SetActive(true);
            }

            if (card.sellable)
            {
                sellable.SetActive(true);
                price.text = card.price.ToString();
            }
        }
        
        private void UpdateRenderID(int id)
        {
            background.sortingOrder = id;
            bord.sortingOrder = id;
            cardName.sortingOrder = id;
            artwork.sortingOrder = id;
            typeName.sortingOrder = id;
            energyText.sortingOrder = id;
            energyIcon.sortingOrder = id;
            noEnergyBar.sortingOrder = id;
            noEnergyIcon.sortingOrder = id;
            slotIcon.sortingOrder = id;
            slotText.sortingOrder = id;
            shadow.sortingOrder = id - 1;
            priceIcon.sortingOrder = id;
            price.sortingOrder = id;
            speedIcon.sortingOrder = id;
            growIcon.sortingOrder = id;
            stockIcon.sortingOrder = id;
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
            if (card.requiereEnergy)
            {
                energyText.text = card.actualEnergy + "/" + card.maxEnergy;
                noEnergy.SetActive(card.actualEnergy < card.energyCost);
            }

            if (parent == null && dragged)
            {
                if (GameManager.instance._draggedCardUI != this)
                {
                    
                    CardUtils.ApplyMethodOnStack(this, c => c.dragged = false);
                    CardUtils.ApplyMethodOnStack(this, card => card.CardDrop());
                    CardUtils.ApplyMethodOnStack(this, c => c.ChangeCollider(true));
                }
            }

            if (card.rateLevel > 0)
            {
                speedIcon.enabled = true;
            }
            if (card.storageLevel > 0)
            {
                stockIcon.enabled = true;
            }
            if (card.productivityLevel > 0)
            {
                growIcon.enabled = true;
            }
        }

        public float pushForce = 1.0f;
        public LayerMask cardLayer;

        public void PushCard(Vector2 force)
        {
            float maxX = GameManager.instance.maxX;
            float maxY = GameManager.instance.maxY;
            
            
            transform.Translate(force);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -maxX, maxX),
                Mathf.Clamp(transform.position.y, -maxY, maxY), transform.position.z);
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

                    Vector2 pushDirection = transform.position - collider.transform.position - new Vector3(0.01f, 0.01f, 0);
                    pushDirection.Normalize();
                    float forcePercent = Mathf.Clamp01(Vector2.Distance(otherCard.transform.position, transform.position));

                    CardUtils.GetRootCard(this).PushCard(pushDirection * (pushForce * forcePercent));
                }

                if(Input.GetMouseButton(0))
                    return;
                
                if (collider.CompareTag("Sell"))
                {
                    Debug.Log("test");
                    SellZone s = collider.GetComponent<SellZone>();
                    SellCard(s);
                }

                if (collider.CompareTag("Buy") && ID == 1)
                {
                    BuyZone b = collider.GetComponent<BuyZone>();
                    BuyCard(b);
                }
                else if(collider.CompareTag("Buy"))
                {
                    Vector3 p = collider.transform.position;
                    p.y -= 5;
                    transform.position = p;
                }
            }
        }
        private void SellCard(SellZone s)
        {
            List<CardUI> stackCards = CardUtils.GetStackCardList(this);

            if (stackCards.All(c => c.card.sellable))
            {
                int totalNumberOfCards = 0;

                foreach (CardUI stackCard in stackCards)
                {
                    totalNumberOfCards += stackCard.card.price;
                }
                
                s.Sell(totalNumberOfCards);
                CardUtils.ApplyMethodOnStack(this, c => Destroy(c.gameObject));
            }
        }

        private void BuyCard(BuyZone buyZone)
        {
            if (AllCardsInStackHaveID(1))
            {
                List<CardUI> stack = CardUtils.GetStackCardList(this);

                foreach (CardUI c in stack)
                {
                    if (buyZone.actualPrice > 0)
                    {
                        buyZone.Buy();
                        if (c.child != null)
                        {
                            c.child.parent = null;
                            Vector3 p = c.child.transform.position;
                            p.y -= 4;
                            c.child.transform.position = p;
                        }

                        Destroy(c.gameObject);
                    }
                    else
                        return;
                }
            }
            else
            {
                Vector3 p = collider.transform.position;
                p.y -= 4;
                transform.position = p;
            }
        }
        private bool AllCardsInStackHaveID(int targetID)
        {
            List<CardUI> cards = CardUtils.GetStackCardList(this);

            foreach (CardUI c in cards)
            {
                if (c.card.ID != targetID)
                {
                    return false;
                }
            }

            return true;
        }
        
        private bool AllCardsInStackSellable()
        {
            List<CardUI> cards = CardUtils.GetStackCardList(this);

            foreach (CardUI c in cards)
            {
                if (!c.card.sellable)
                {
                    return false;
                }
            }
            return true;
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
        
        public void CardEnter()
        {
            Vector3 p = model.transform.localPosition;
            model.transform.localPosition = new Vector3(p.x, 0.1f, p.z);
            behavior?.OnHover();
        }
        public void CardExit()
        {
            Vector3 p = model.transform.localPosition;
            model.transform.localPosition = new Vector3(p.x, 0, p.z);
        }

        public void CardDrag()
        {
            Vector3 p = model.transform.localPosition;
            model.transform.localPosition = new Vector3(p.x, 0.2f, p.z);
        }

        public void CardDrop()
        {
            Vector3 p = model.transform.localPosition;
            model.transform.localPosition = new Vector3(p.x, 0f, p.z);
        }

        private void OnMouseOver()
        {
            behavior?.OnHoverStay();
        }

        private void OnDestroy()
        {
            if(loader != null)
                Destroy(loader);
            if(child != null)
                child.SetParent(parent);
        }
        
        
    }
}