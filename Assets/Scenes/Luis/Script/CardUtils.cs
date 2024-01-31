using System;
using System.Collections.Generic;
using Leafy.Objects;

namespace Leafy.Data
{
    public static class CardUtils
    {
        /// <summary>
        /// Return the list of ID of the card stack
        /// </summary>
        /// <param name="cardUI"></param>
        /// <returns></returns>
        public static List<int> GetStackIDList(CardUI cardUI)
        {
            CardUI c = GetRootCard(cardUI);
            List<int> IDs = new List<int>();

            do
            {
                IDs.Add(c.ID);
                c = c.child;
            } while (c != null);

            return IDs;
        }
        
        /// <summary>
        /// Return the list of Card class of the card stack
        /// </summary>
        /// <param name="cardUI"></param>
        /// <returns></returns>
        public static List<CardUI> GetStackCardList(CardUI cardUI)
        {
            CardUI c = GetRootCard(cardUI);
            List<CardUI> cards = new List<CardUI>();

            do
            {
                cards.Add(c);
                c = c.child;
            } while (c != null);

            return cards;
        }
        
        /// <summary>
        /// Return the first card of the stack
        /// </summary>
        /// <param name="cardUI"></param>
        /// <returns></returns>
        public static CardUI GetRootCard(CardUI cardUI)
        {
            if (cardUI == null)
                return null;
            
            CardUI c = cardUI;
            while (c.parent != null)
            {
                c = c.parent;
            }
            return c;
        }
        
        
        /// <summary>
        /// Return the last card of the stack
        /// </summary>
        /// <param name="cardUI"></param>
        /// <returns></returns>
        public static CardUI GetLastCard(CardUI cardUI)
        {
            if (cardUI == null)
                return null;
            
            CardUI c = cardUI;
            while (c.child != null)
            {
                c = c.child;
            }
            return c;
        }

        /// <summary>
        /// Apply a methods on each card of the stack
        /// </summary>
        /// <param name="cardUI"></param>
        /// <param name="method"></param>
        public static void ApplyMethodOnStack(CardUI cardUI, Action<CardUI> method)
        {
            List<CardUI> cards = GetStackCardList(cardUI);

            foreach (CardUI c in cards)
            {
                method(c);
            }
        }
        
    }
}