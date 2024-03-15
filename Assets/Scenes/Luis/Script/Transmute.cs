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

        public Transmute(CardUI cardUI) : base()
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
                if (cardUI.child.ID == 39 && card.requiereEnergy)
                {
                    if (card.actualEnergy < card.maxEnergy)
                    {
                        card.actualEnergy++;
                        GameManager.instance.DestroyObject(cardUI.child.gameObject);
                    }
                }
                
                List<CardUI> childList = CardUtils.GetChildCardList(cardUI.child);
    
                if (card.transmuteRecipes != null)
                {
                    //Test All stack
                    foreach (TransmuteRecipe recipe in card.transmuteRecipes)
                    {
                        List<CardUI> c = IsRecipeInChildStack(childList, recipe);
                        if (c != null)
                        {
                            if (card.requiereEnergy)
                            {
                                if(card.actualEnergy < card.energyCost)
                                    return;
                            }

                            List<int> ids = new List<int>();
                            
                            for (var i = 0; i < recipe.resultAmount; i++)
                            {
                                if (recipe.result.TryPickValue(out var scriptableCard))
                                {
                                    ids.Add(scriptableCard.ID);
                                }
                                else
                                {
                                    Debug.Log("No item was selected.");
                                }
                                // ids.Add(recipe.result.PickValue().ID);
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

            if (remainingCards.Count < recipe.recipe.Count)
            {
                return null;
            }

            var firstNCards = remainingCards.Take(recipe.recipe.Count);

            var firstNCardsIDs = firstNCards.Select(card => card.ID).OrderBy(id => id).ToList();
            var recipeIDs = recipe.recipe.OrderBy(id => id).ToList();

            bool cardsMatch = firstNCardsIDs.SequenceEqual(recipeIDs);

            if (cardsMatch)
            {
                return firstNCards.ToList();
            }
            
            return null;
            
        }
        
    }
}