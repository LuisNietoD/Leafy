using System.Collections.Generic;
using System.Linq;
using Leafy.Data;
using Leafy.Manager;
using UnityEngine;

namespace Leafy.Objects
{
    public class Transmute: CardBehavior
    {
        private CardUI cardUI;
        private Card card;

        public Transmute(CardUI cardUI)
        {
            this.cardUI = cardUI;
            card = this.cardUI.card;
        }
        
        public override void Spawn()
        {
        }

        public override void OnDrag()
        {
        }

        public override void OnDrop()
        {
        }

        public override void OnClick()
        {
            
        }

        public override void OnHover()
        {
            Debug.Log("OnHover");
        }

        public override void OnHoverStay()
        {
            Debug.Log("OnHoverStay");
        }
        
        public override void StayAction()
        {
            if(cardUI.child == null && cardUI.loader != null)
                GameManager.instance.DestroyObject(cardUI.loader);
            
            if (cardUI.child != null && cardUI.loader == null)
            {
                List<CardUI> childList = CardUtils.GetChildCardList(cardUI.child);
    
                if (card.transmuteRecipes != null)
                {
                    //Test All stack
                    foreach (TransmuteRecipe recipe in card.transmuteRecipes)
                    {
                        List<CardUI> c = IsRecipeInChildStack(childList, recipe);
                        if (c != null)
                        {
                            List<int> ids = new List<int>();
                            for (int i = 0; i < recipe.resultAmount; i++)
                            {
                                ids.Add(recipe.result.PickValue().ID);
                            }
                            GameManager.instance.LaunchTransmute(cardUI, ids, c, 2);
                        }
                    }
                }
            }
        }
        
        private List<CardUI> IsRecipeInChildStack(List<CardUI> childStack, TransmuteRecipe recipe)
        {
            var remainingCards = new List<CardUI>(childStack);

            // Check if there are enough remaining cards to match the recipe size
            if (remainingCards.Count < recipe.recipe.Count)
            {
                return null; // Not enough cards, return null
            }

            // Take the first 'recipe.recipe.Count' cards from the remaining cards
            var firstNCards = remainingCards.Take(recipe.recipe.Count);

            // Extract IDs from firstNCards and recipe and sort them
            var firstNCardsIDs = firstNCards.Select(card => card.ID).OrderBy(id => id).ToList();
            var recipeIDs = recipe.recipe.OrderBy(id => id).ToList();

            // Check if the sorted IDs match
            bool cardsMatch = firstNCardsIDs.SequenceEqual(recipeIDs);

            if (cardsMatch)
            {
                return firstNCards.ToList(); // Return the list of matching cards
            }
            
            return null; // If the cards don't match, return null
            
        }
        
    }
}