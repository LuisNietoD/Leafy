using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leafy.Data;
using Leafy.Manager;
using Leafy.Objects;
using UnityEngine;


namespace Leafy.Objects
{
    public class AssembleurInterface : CardGameInterface
    {
        public Slot slot;

        public override void InterfaceAction()
        {
            if (slot.transform.childCount <= 0)
                return;

            CardUI cardUI = slot.transform.GetChild(0).GetComponent<StackParent>().inStack[0];

            Vector3 p = slot.transform.position;
            p.x += 2;

            List<CardUI> childList = CardUtils.GetStackCardList(cardUI.child);

            foreach (var recipe in Craft.assembleur)
            {

                List<CardUI> c = IsRecipeInChildStack(childList, recipe.Value);
                if (c != null)
                {
                    GameManager.instance.SpawnCard(p, recipe.Key);
                    foreach (CardUI card in c)
                    {
                        Destroy(card.gameObject);
                    }

                    return;
                }
            }
        }

        private List<CardUI> IsRecipeInChildStack(List<CardUI> childStack, List<int> recipe)
        {
            var remainingCards = new List<CardUI>(childStack);

            if (remainingCards.Count < recipe.Count)
            {
                return null;
            }

            var firstNCards = remainingCards.Take(recipe.Count);

            var firstNCardsIDs = firstNCards.Select(card => card.ID).OrderBy(id => id).ToList();
            var recipeIDs = recipe.OrderBy(id => id).ToList();

            bool cardsMatch = firstNCardsIDs.SequenceEqual(recipeIDs);

            if (cardsMatch)
            {
                return firstNCards.ToList();
            }

            return null;

        }
    }
}
