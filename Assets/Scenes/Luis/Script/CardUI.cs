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
        [ES3Serializable]
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
        private SpriteRenderer artworkShadow;
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

        public CardUI parent;
        public CardUI child;
        public int ID = -1;

        private GameObject _interface;

        public Vector3 startPosition;
        public float animTime = 0.7f;
        public float elapsedTime = 0;
        private bool halfway = false;
        private Vector3 originalScale;
        public bool halfTime;


        //private CardBehavior cardBehavior;
        private bool hovered;
        public bool dragged;

        private static int BuyPack = 0;
        
        private void Update()
        {
            if (elapsedTime < animTime/2)
            {
                elapsedTime += Time.deltaTime;

                background.material.SetFloat("_Dissolve", 1.1f - ((elapsedTime/animTime) * 1.1f));
            }
            else if (elapsedTime < animTime && elapsedTime >= animTime / 2)
            {
                elapsedTime += Time.deltaTime;

                float r = Mathf.Lerp(-180, 0, (elapsedTime-animTime/2)/(animTime/2));
                if (r > -90 && !bord.gameObject.activeSelf)
                {
                    UpdateCardInfo(card);
                }
                model.transform.localEulerAngles = new Vector3(0, r, 0);
                shadow.transform.localEulerAngles = new Vector3(0, r, 0);
                background.material.SetFloat("_Dissolve", 1.1f - ((elapsedTime/animTime) * 1.1f));

            }
            else if (elapsedTime > animTime)
            {
                model.transform.localEulerAngles = new Vector3(0, 0, 0);
                shadow.transform.localEulerAngles = new Vector3(0, 0, 0);
                shadow.gameObject.SetActive(true);
                if (!bord.gameObject.activeSelf)
                {
                    UpdateCardInfo(card);
                }
            }
        }

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            cardName = transform.Find("MODEL/CARD NAME").GetComponent<TextMeshPro>();
            typeName = transform.Find("MODEL/TYPE").GetComponent<TextMeshPro>();
            artwork = transform.Find("MODEL/ICON").GetComponent<SpriteRenderer>();
            artworkShadow = transform.Find("MODEL/ICONSHADOW").GetComponent<SpriteRenderer>();
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
            uniqueID = CardUtils.ID++;
            
            if(card != null)
                UpdateCardInfo(card);

            SpawnCollision();
            if (transform.parent == null)
            {
                GameManager.instance.SetToParent(this);
            }
            behavior?.Spawn();

            foreach (Transform child in model.transform)
            {
                child.gameObject.SetActive(false);
            }
            background.gameObject.SetActive(true);
            shadow.gameObject.SetActive(false);
            if (card.ID == 1)
            {
                QuestManager.instance.UpdateQuest(2);
            }

        }

        public void UpdateCardInfo(Card c)
        {
            card = c;
            cardName.text = card.name;
            cardName.gameObject.SetActive(true);
            typeName.text = card.type;
            typeName.gameObject.SetActive(true);
            background.sprite = card.background;
            artwork.sprite = card.artwork;
            artworkShadow.sprite = card.artwork;
            artwork.gameObject.SetActive(true);
            artworkShadow.gameObject.SetActive(true);
            bord.gameObject.SetActive(true);
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
                if (interfaceSlot == null)
                {
                    GameObject g = Instantiate(i, transform.position, Quaternion.identity);
                    g.transform.SetParent(transform);
                    g.transform.localPosition = Vector3.zero;
                    g.name = "INTERFACE";

                    interfaceSlot = g;
                }
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
            artworkShadow.sortingOrder = id;
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
        
        public void UpdateRenderLayer(int value)
        {
            background.sortingLayerID = value;
            bord.sortingLayerID = value;
            cardName.sortingLayerID = value;
            artwork.sortingLayerID = value;
            artworkShadow.sortingLayerID = value;
            typeName.sortingLayerID = value;
            energyText.sortingLayerID = value;
            energyIcon.sortingLayerID = value;
            noEnergyBar.sortingLayerID = value;
            noEnergyIcon.sortingLayerID = value;
            slotIcon.sortingLayerID = value;
            slotText.sortingLayerID = value;
            shadow.sortingLayerID = value;
            priceIcon.sortingLayerID = value;
            price.sortingLayerID = value;
            speedIcon.sortingLayerID = value;
            growIcon.sortingLayerID = value;
            stockIcon.sortingLayerID = value;
        }

        /*private void Update()
        {
            if (elapsedTime < animTime)
            {
                elapsedTime += Time.deltaTime;
                Vector3 p = Vector3.Lerp(startPosition, Vector3.zero, elapsedTime / animTime);
                p.z = 1;
                model.transform.localPosition = p;
                shadow.gameObject.transform.localPosition = new Vector3(model.transform.localPosition.x - 0.048f,
                    model.transform.localPosition.y - 0.048f, 1);
                rotation = Mathf.Lerp(-180, 0, elapsedTime / animTime);
                Vector3 rot = new Vector3(0, rotation, 0);
                model.transform.localEulerAngles = rot;
                shadow.transform.localEulerAngles = rot;

                if (rot.y > -90 && !artwork.gameObject.activeSelf)
                {
                    UpdateCardInfo(card);
                }
            }
            else
            {
                model.transform.localPosition = new Vector3(0, 0, 1);
                shadow.gameObject.transform.localPosition = new Vector3( -0.048f, -0.048f, 1);
                model.transform.localEulerAngles = new Vector3(0, 0, 0);
                shadow.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            
        }*/

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
            if(!collide)
                return;
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
            }
        }
        
        private void SpawnCollision()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0f, cardLayer);

            foreach (var collider in colliders)
            {
                CardUI otherCard = collider.GetComponent<CardUI>();

                if (otherCard != null && otherCard != this && !dragged && !CardUtils.IsInTheSameStack(this, otherCard))
                {
                    if (uniqueID > otherCard.uniqueID)
                    {
                        if (parent == null)
                        {
                            SetParent(CardUtils.GetLastCard(otherCard));
                            CardUtils.ApplyMethodOnStack(this, c => c.ChangeID(GameManager.instance.ID++));
                            GameManager.instance.SetToParent(this);
                            return;
                        }
                        
                        if (child == null && otherCard.parent == null)
                        {
                            otherCard.SetParent(this);
                            CardUtils.ApplyMethodOnStack(this, c => c.ChangeID(GameManager.instance.ID++));
                            GameManager.instance.SetToParent(this);
                            return;
                        }
                    }
                }
            }
        }

        private bool collide = true;
        
        private void TestSellCollision()
        {
            collide = false;
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0f, cardLayer);

            foreach (var collider in colliders)
            {
                switch (collider.tag)
                {
                    case "Sell":
                        SellZone s = collider.GetComponent<SellZone>();
                        SellCard(s);
                        break;
                    
                    case "Buy":
                        BuyZone b = collider.GetComponent<BuyZone>();
                        BuyCard(b);
                        break;
                    
                    case "Recycle":
                        RecycleZone r = collider.GetComponent<RecycleZone>();
                        RecycleCard(r);
                        break;
                    
                    case "HUD":
                        transform.position = CameraCenterToPoint();
                        break;
                    
                    case "Blueprint":
                        BlueprintManager blueprintManager = collider.GetComponent<BlueprintManager>();
                        Blueprint(blueprintManager);
                        break;
                    case "Craft":
                        CraftZone craftZone = collider.GetComponent<CraftZone>();
                        AddToCraftCard(craftZone);
                        break;
                }
                
                collide = true;
                return;

                if (collider.CompareTag("Sell"))
                {
                    Debug.Log("test");
                    SellZone s = collider.GetComponent<SellZone>();
                    SellCard(s);
                    return;
                }

                if (collider.CompareTag("Buy") && ID == 1)
                {
                    BuyZone b = collider.GetComponent<BuyZone>();
                    BuyCard(b);
                    return;
                }

                if (collider.CompareTag("Recycle") && card.type == "Plant")
                {
                    RecycleZone r = collider.GetComponent<RecycleZone>();
                    RecycleCard(r);
                    return;
                }
                
                if(collider.CompareTag("Buy"))
                {
                    Vector3 p = collider.transform.position;
                    p.y -= 5;
                    transform.position = p;
                    return;
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
                CardUtils.ApplyMethodOnStack(this, c =>
                {
                    QuestManager.instance.UpdateQuest(3);
                    Destroy(c.gameObject);
                });
            }
            else
            {
                transform.position = CameraCenterToPoint();
            }
        }
        
        private void RecycleCard(RecycleZone r)
        {
            List<CardUI> stackCards = CardUtils.GetStackCardList(this);

            if (stackCards.All(c => c.card.type == "Plant"))
            {
                int totalNumberOfCards = stackCards.Count;
                r.Recycle(totalNumberOfCards);
                CardUtils.ApplyMethodOnStack(this, c => Destroy(c.gameObject));
            }
            else
            {
                transform.position = CameraCenterToPoint();
            }
        }

        public void Blueprint(BlueprintManager blueprintManager)
        {
            if (AllCardsInStackHaveID(51))
            {
                List<CardUI> stack = CardUtils.GetStackCardList(this);

                foreach (CardUI c in stack)
                {
                    if (blueprintManager.gameObject.activeSelf)
                    {
                        blueprintManager.EnableNextRecipe();
                        Destroy(c.gameObject);
                    }
                    else
                    {
                        transform.position = CameraCenterToPoint();
                    }
                }
            }
            else
            {
                transform.position = CameraCenterToPoint();
            }
        }
        
        private void AddToCraftCard(CraftZone r)
        {
            List<CardUI> stack = CardUtils.GetStackCardList(this);

            r.AddCard(stack);
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
                transform.position = CameraCenterToPoint();
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
            TestSellCollision();
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
                if ((card.harvestable && c.card.ID == card.ID) || !card.harvestable && card.ID != 11)
                    parent = c;
                else
                {
                    parent = null;
                }
            }
            else
            {
                parent = null;
            }


            if (parent != null)
                parent.child = this;
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
            if (transform.parent != null)
            {
                if (transform.parent.TryGetComponent(out StackParent stackParent))
                {
                    stackParent.inStack.Remove(this);
                }
            }
        }

        public Vector3 CameraCenterToPoint()
        {
            if (Camera.main != null)
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 rayDirection = mouseWorldPosition - Camera.main.transform.position;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, rayDirection, Mathf.Infinity, terrainLayer);

                if (hit.collider != null)
                {
                    Vector3 spawnPoint = hit.point;
                    spawnPoint.z = 0;
                    //spawnPoint.x += 5;
                    return spawnPoint;
                }
                else
                {
                    Vector2 nearestPoint = FindNearestPointOnTerrain(Camera.main.transform.position, rayDirection);
                    Vector3 spawnPoint = nearestPoint;
                    spawnPoint.z = 0;
                    //spawnPoint.x += 5;
                    return spawnPoint;
                }
                
            }

            return Vector3.zero;
        }

        public LayerMask terrainLayer;
        private Vector2 FindNearestPointOnTerrain(Vector3 position, Vector3 direction)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 50f, terrainLayer);

            if (colliders.Length > 0)
            {
                Vector2 nearestPoint = colliders[0].ClosestPoint(position);
                float nearestDistance = Vector2.Distance(position, nearestPoint);

                foreach (Collider2D collider in colliders)
                {
                    Vector2 point = collider.ClosestPoint(position);
                    float distance = Vector2.Distance(position, point);
                
                    if (distance < nearestDistance)
                    {
                        nearestPoint = point;
                        nearestDistance = distance;
                    }
                }

                return nearestPoint;
            }

            
            // If no terrain collider found, return current position
            return position;
        }
    }
}