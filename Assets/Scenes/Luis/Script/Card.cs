using UnityEngine;

namespace Leafy.Data
{
    public class Card
    {
        public string name;
        public Sprite artwork;
        public Color backgroundColor;
        public int ID;

        public Card(ScriptableCard card)
        {
            name = card.name;
            artwork = card.artwork;
            backgroundColor = card.background_color;
            ID = card.ID;
        }
    }
}