using Leafy.Objects;
using UnityEngine;

namespace Leafy.Objects
{
    public class ClassicCard: CardBehavior
    {
        public override void Spawn()
        {
            Debug.Log("spawn");
        }

        public override void OnDrag()
        {
            Debug.Log("OnDrag");
        }

        public override void OnDrop()
        {
            Debug.Log("OnDrop");
        }

        public override void OnClick()
        {
            Debug.Log("OnClick");
        }

        public override void OnHover()
        {
            Debug.Log("OnHover");
        }

        public override void OnHoverStay()
        {
            Debug.Log("OnHoverStay");
        }
    }
}