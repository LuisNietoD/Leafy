using System.Collections.Generic;
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

        private void Update()
        {
            _hoveredCardUI = RayTestCard();

            //Grab the hovered card and drag it
            if (Input.GetMouseButtonDown(0) && _hoveredCardUI != null)
            {
                offset = _hoveredCardUI.SetDragged();
                _draggedCardUI = _hoveredCardUI;
                CardUtils.ApplyMethodOnStack(_draggedCardUI, card =>
                {
                    card.BringToFront();
                    card.ChangeID(ID++);
                });
                _hoveredCardUI = null;
                //add event for update craft loader
            }
            //Drop the dragged card
            else if (Input.GetMouseButtonUp(0) && _draggedCardUI != null)
            {
                
                _draggedCardUI.Drop(_hoveredCardUI);
                int craft = Craft.GetCraft(CardUtils.GetStackIDList(_draggedCardUI));
                if (craft >= 0)
                {
                    LaunchCraft(craft, CardUtils.GetRootCard(_draggedCardUI));
                }

                CardUtils.ApplyMethodOnStack(_draggedCardUI, c => c.dragged = false);
                _draggedCardUI = null;
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                List<ScriptableCard> c = CardList.GetRandomCardList(8);
              
                Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                

                foreach (ScriptableCard card in c)
                {
                    GameObject myCard = Instantiate(cardPrefab, p, Quaternion.identity);
                    if (myCard.TryGetComponent(out CardUI cardUI))
                    {
                        cardUI.UpdateCardInfo(new Card(card));
                        if(lastCard != null)
                            cardUI.SetParent(lastCard);
                        SetLastCard(cardUI);
                    }

                    myCard.transform.position = new Vector3(p.x, p.y, 0);
                }

                lastCard = null;
                c.Clear();
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

        
        public void TestCraft(CardUI cardUI)
        {
            CardUI firstStackCardUI = CardUtils.GetRootCard(cardUI);
            int craft = Craft.GetCraft(CardUtils.GetStackIDList(firstStackCardUI));
            
            if (craft >= 0 && firstStackCardUI.loader == null)
            {
                LaunchCraft(craft, firstStackCardUI);
            }
        }

        private void LaunchCraft(int craftID, CardUI firstStackCardUI)
        {
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

            
            cl.stack[0].SetLoader(cl.gameObject);
            
        }
        
        public void LaunchCraft(int craftID, List<CardUI> stack)
        {
            ScriptableCard toCraft = CardList.GetCardByID(craftID);
            if(toCraft == null)
                Debug.LogError("Cannot find this craft ID");

            Vector3 pos = stack[0].transform.position;
            pos.y += crafterOffsetY;
            GameObject crafter = Instantiate(crafterPrefab, pos, Quaternion.identity);

            CraftLoading cl = crafter.GetComponent<CraftLoading>();

            cl.timeToCraft = toCraft.timeToCraft;
            cl.drop = toCraft;
            cl.stack = stack;

            cl.stack[0].SetLoader(cl.gameObject);
        }

        CardUI lastCard;

        private void SetLastCard(CardUI c)
        {
            lastCard = c;
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
                    return cardUI;
                }
                else if(hit.collider.TryGetComponent(out Booster b))
                {
                    if(Input.GetMouseButtonDown(0))
                        b.SpawnCard();
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
    }
}

