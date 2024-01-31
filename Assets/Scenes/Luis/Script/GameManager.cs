using Leafy.Objects;
using UnityEngine;
using Leafy.Data;

namespace Leafy.Manager
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager instance;
        
        public LayerMask cardLayer;
        
        private CardUI _draggedCardUI;
        private CardUI _hoveredCardUI;

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
                
                _draggedCardUI = null;
            }
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

            foreach (CardUI c in cl.stack)
            {
                c.SetLoader(cl.gameObject);
            }
        }

        private void FixedUpdate()
        {
            //Drag the card with a little delay
            if (_draggedCardUI != null)
            {
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                Vector2 position = Vector2.Lerp(_draggedCardUI.transform.position, targetPosition, draggedCardSpeed * Time.deltaTime);
                float z = frontViewZ;
                _draggedCardUI.transform.position = new Vector3(position.x, position.y, z);
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
                CardUI cardUI = hit.collider.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    return cardUI;
                }
            }
            return null;
        }
    }
}

