using System;
using System.Collections.Generic;
using Leafy.Objects;
using Unity.VisualScripting;

namespace Leafy.Data
{
    public static class CardUtils
    {
        public static int ID = 0;
        
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
        /// If the first parameter is a parent of the second parameter
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool IsParentOf(CardUI parent, CardUI child)
        {
            CardUI c = child;
            do
            {
                if (c == parent)
                    return true;
                c = c.parent;
            } while (c != null);

            return false;
        }

        /// <summary>
        /// Return true if this two card are in the same stack
        /// </summary>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        /// <returns></returns>
        public static bool IsInTheSameStack(CardUI card1, CardUI card2)
        {
            return GetStackCardList(card1).Contains(card2);
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