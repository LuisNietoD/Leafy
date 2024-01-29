using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Leafy.Objects
{
    public static class CardUtils
    {
        public static List<int> GetStackIDList(Card card)
        {
            Card c = GetRootCard(card);
            List<int> IDs = new List<int>();

            do
            {
                IDs.Add(c.ID);
                c = c.child;
            } while (c != null);

            return IDs;
        }
        
        public static List<Card> GetStackCardList(Card card)
        {
            Card c = GetRootCard(card);
            List<Card> cards = new List<Card>();

            do
            {
                cards.Add(c);
                c = c.child;
            } while (c != null);

            return cards;
        }

        public static Card GetRootCard(Card card)
        {
            if (card == null)
                return null;
            
            Card c = card;
            while (c.parent != null)
            {
                c = c.parent;
            }
            return c;
        }
        
        public static Card GetLastCard(Card card)
        {
            if (card == null)
                return null;
            
            Card c = card;
            while (c.child != null)
            {
                c = c.child;
            }
            return c;
        }

        public static void ApplyMethodOnStack(Card card, Action<Card> method)
        {
            List<Card> cards = GetStackCardList(card);

            foreach (Card c in cards)
            {
                method(c);
            }
        }
    }
}