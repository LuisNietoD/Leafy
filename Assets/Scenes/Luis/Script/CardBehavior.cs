using Unity.VisualScripting;
using UnityEngine;

namespace Leafy.Objects
{
    public abstract class CardBehavior
    {
        public abstract void Spawn();
        public abstract void OnDrag();
        public abstract void OnDrop();
        public abstract void OnClick();
        public abstract void OnHover();
        public abstract void OnHoverStay();
        public abstract void StayAction();
        
        
    }
}