using System.Collections.Generic;
using System.Xml;
using Leafy.Objects;
using UnityEngine;
using Leafy.Data;

namespace Leafy.Manager
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager instance;
        
        public LayerMask cardLayer;
        public GameObject cardPrefab;
        
        
        public float snapX = 1;
        public float snapY = 2;
        private float lerpingSpeed = 30f;
        
        private CardUI _draggedCardUI;
        private CardUI _hoveredCardUI;

        public int ID = 0;
        
        private float draggedCardSpeed = 10;
        private Vector3 offset;
        private float frontViewZ = -9;

        public GameObject crafterPrefab;
        private float crafterOffsetY = 2;

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

        private float time;

        private void Update()
        {
            _hoveredCardUI = RayTestCard();

            //Grab the hovered card and drag it
            if (Input.GetMouseButtonDown(0) && _hoveredCardUI != null)
            {
                offset = _hoveredCardUI.SetDragged();
                _hoveredCardUI.transform.parent = null;
                _draggedCardUI = _hoveredCardUI;
                _draggedCardUI.behavior?.OnDrag();
                CardUtils.ApplyMethodOnStack(_draggedCardUI, card =>
                {
                    card.BringToFront();
                    card.ChangeID(ID++);
                });
                _hoveredCardUI = null;
                CardUtils.ApplyMethodOnAllChild(_draggedCardUI, card => card.CardDrag());
                //add event for update craft loader
            }
            //Drop the dragged card
            else if (Input.GetMouseButtonUp(0) && _draggedCardUI != null)
            {
                Transform slot = RayTestUI();
                if (slot != null)
                {
                    _draggedCardUI.transform.parent = slot;
                }
                
                _draggedCardUI.behavior?.OnDrop();
                _draggedCardUI.Drop(_hoveredCardUI);
                

                int craft = Craft.GetCraft(CardUtils.GetStackIDList(_draggedCardUI));
                if (craft >= 0)
                {
                    LaunchCraft(craft, CardUtils.GetRootCard(_draggedCardUI));
                }

                CardUtils.ApplyMethodOnStack(_draggedCardUI, c => c.dragged = false);
                CardUtils.ApplyMethodOnAllChild(_draggedCardUI, card => card.CardDrop());

                
                if (Input.GetMouseButtonUp(0))
                {
                    if (time <= 0.8f)
                    {
                        _draggedCardUI.behavior?.OnClick();
                        time = 0;
                    }
                }
                
                _draggedCardUI = null;
            }
            
            if (Input.GetMouseButton(0))
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
            }
            
        }

        private void SnapCardToGrid()
        {
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float snappedX = Mathf.Round(mousePosition.x / snapX) * snapX;
            float snappedY = Mathf.Round(mousePosition.y / snapY) * snapY;

            Vector3 finalSnappedPosition = new Vector3(snappedX, snappedY, 0f);

            // Lerp towards the snapped position
            _draggedCardUI.transform.position = Vector3.Lerp(_draggedCardUI.transform.position, finalSnappedPosition, Time.deltaTime * lerpingSpeed);
        
        }

        private void LaunchCraft(int craftID, CardUI firstStackCardUI)
        {
            if(firstStackCardUI.loader != null)
                return;
            ScriptableCard toCraft = CardList.GetCardByID(craftID);
            if(toCraft == null)
                Debug.LogError("Cannot find this craft ID");

            Vector3 pos = firstStackCardUI.transform.position;
            pos.y += crafterOffsetY;
            GameObject crafter = Instantiate(crafterPrefab, pos, Quaternion.identity);

            CraftLoading cl = crafter.GetComponent<CraftLoading>();

            cl.timeToCraft = toCraft.timeToCraft;
            cl.drop = toCraft;
            cl.stack = CardUtils.GetStackCardList(firstStackCardUI);
            cl.destroyStack = true;

            
            cl.stack[0].SetLoader(cl.gameObject);
            
        }
        
        public void LaunchCraft(int craftID, List<CardUI> stack, float harvestTime)
        {
            if(stack[0].loader != null)
                return;
            ScriptableCard toCraft = CardList.GetCardByID(craftID);
            if(toCraft == null)
                Debug.LogError("Cannot find this craft ID");

            Vector3 pos = stack[0].transform.position;
            pos.y += crafterOffsetY;
            GameObject crafter = Instantiate(crafterPrefab, pos, Quaternion.identity);

            CraftLoading cl = crafter.GetComponent<CraftLoading>();

            cl.timeToCraft = harvestTime;
            cl.drop = toCraft;
            cl.stack = stack;
            

            cl.stack[0].SetLoader(cl.gameObject);
        }
        
        public void LaunchCraft(int craftID, List<CardUI> stack, float harvestTime, bool destroy)
        {
            if(stack[0].loader != null)
                return;
            ScriptableCard toCraft = CardList.GetCardByID(craftID);
            if(toCraft == null)
                Debug.LogError("Cannot find this craft ID");

            Vector3 pos = stack[0].transform.position;
            pos.y += crafterOffsetY;
            GameObject crafter = Instantiate(crafterPrefab, pos, Quaternion.identity);

            CraftLoading cl = crafter.GetComponent<CraftLoading>();

            cl.timeToCraft = harvestTime;
            cl.drop = toCraft;
            cl.stack = stack;
            cl.destroyStack = destroy;
            

            cl.stack[0].SetLoader(cl.gameObject);
        }
        
        public void LaunchTransmute(CardUI transmuter, int cardID, List<CardUI> stackToDestroy, float transmuteTime)
        {
            if(transmuter.loader != null)
                return;
            ScriptableCard toCraft = CardList.GetCardByID(cardID);
            if(toCraft == null)
                Debug.LogError("Cannot find this craft ID" + cardID);

            Vector3 pos = transmuter.transform.position;
            pos.y += crafterOffsetY;
            GameObject crafter = Instantiate(crafterPrefab, pos, Quaternion.identity);

            CraftLoading cl = crafter.GetComponent<CraftLoading>();

            cl.timeToCraft = transmuteTime;
            cl.drop = toCraft;
            cl.stack = stackToDestroy;
            cl.destroyStack = true;
            
            transmuter.SetLoader(cl.gameObject);
        }
        
        public void LaunchTransmute(CardUI transmuter, List<int> cardID, List<CardUI> stackToDestroy, float transmuteTime)
        {
            if(transmuter.loader != null)
                return;

            Vector3 pos = transmuter.transform.position;
            pos.y += crafterOffsetY;
            GameObject crafter = Instantiate(crafterPrefab, pos, Quaternion.identity);

            CraftLoading cl = crafter.GetComponent<CraftLoading>();

            cl.timeToCraft = transmuteTime;
            cl.stack = stackToDestroy;
            cl.destroyStack = true;
            cl.multipleCards = true;
            cl.toCraft = cardID;
            
            transmuter.SetLoader(cl.gameObject);
        }
        
        private void FixedUpdate()
        {
            //Drag the card with a little delay
            if (_draggedCardUI != null)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    SnapCardToGrid();
                }
                else
                {
                    Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                    Vector2 position = Vector2.Lerp(_draggedCardUI.transform.position, targetPosition, draggedCardSpeed * Time.deltaTime);
                    float z = frontViewZ;
                    _draggedCardUI.transform.position = new Vector3(position.x, position.y, z);
                }
            }
        }

        /// <summary>
        /// Return the first card hit on the board
        /// </summary>
        /// <returns></returns>
        public CardUI RayTestCard()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, cardLayer);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out CardUI cardUI))
                {
                    if (cardUI != _hoveredCardUI)
                    {
                        cardUI.CardEnter();
                        if(_hoveredCardUI != null)
                            _hoveredCardUI.CardExit();
                    }

                    return cardUI;
                }
                if(hit.collider.TryGetComponent(out Booster b))
                {
                    if(Input.GetMouseButtonDown(0))
                        b.SpawnCard();
                }
            }

            if (_hoveredCardUI != null)
                _hoveredCardUI.CardExit();
            return null;
        }
        
        public Transform RayTestUI()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, cardLayer);

            if (hit.collider != null)
            {
                if (hit.collider.transform != null)
                {
                    Debug.Log("Transform found: " + hit.collider.transform.name);
                    
                        if (hit.collider.transform.TryGetComponent(out Slot slot))
                        {
                            if (slot.transform.childCount <= 0)
                                return hit.collider.transform;
                    }
                }
            }
            return null;
        }

        
        public void SpawnCard(Vector3 pos, int id, CardUI card)
        {
            GameObject newCard = Instantiate(cardPrefab, pos, Quaternion.identity);
            CardUI c = newCard.GetComponent<CardUI>();
            c.UpdateCardInfo(new Card(CardList.GetCardByID(id)));
            c.ChangeID(ID++);
            card.ChangeID(ID++);
        }
        
        public void SpawnCard(Vector3 pos, int id)
        {
            GameObject newCard = Instantiate(cardPrefab, pos, Quaternion.identity);
            CardUI c = newCard.GetComponent<CardUI>();
            c.UpdateCardInfo(new Card(CardList.GetCardByID(id)));
            c.ChangeID(ID++);
        }
        
        public CardUI SpawnCardRef(Vector3 pos, int id)
        {
            GameObject newCard = Instantiate(cardPrefab, pos, Quaternion.identity);
            CardUI c = newCard.GetComponent<CardUI>();
            c.UpdateCardInfo(new Card(CardList.GetCardByID(id)));
            c.ChangeID(ID++);
            return c;
        }

        public void SpawnStack(Vector3 pos, int id, int stackSize)
        {
            CardUI parent = SpawnCardRef(pos, id);

            for (int i = 0; i < stackSize-1; i++)
            {
                CardUI c = SpawnCardRef(pos, id);
                c.SetParent(parent);
                parent = c;
            }
        }

        public void SpawnStackPrecise(Vector3 pos, List<int> ids)
        {
            CardUI parent = SpawnCardRef(pos, ids[0]);
            
            for (int i = 1; i < ids.Count; i++)
            {
                CardUI c = SpawnCardRef(pos, ids[i]);
                c.SetParent(parent);
                parent = c;
            }
        }

        public GameObject SpawnObject(Vector3 pos, GameObject prefab)
        {
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            return obj;
        }

        public void DestroyObject(GameObject o)
        {
            Destroy(o);
        }
    }
}

