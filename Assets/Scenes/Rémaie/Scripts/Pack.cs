using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

[ExecuteInEditMode]
public class CardsManager : MonoBehaviour
{
    public float tokens = 50;
    public TextMeshProUGUI consoleText;
    public TextMeshProUGUI rareCardsText;
    public GameObject[] cardsPrefabs;
    public float distanceSpawnInFrontOfCamera = 5f;
    public RawImage Mask1;
    public RawImage Mask2;
    public RawImage Mask3;
    public RawImage Mask4;
    public RawImage Mask5;
    public RawImage Mask6;
    public RawImage Mask7;

    // List of different cards
    private List<string> cards = new List<string>()
    {
        "Blue Card",
        "Yellow Card",
        "Orange Card",
        "Pink Card",
        "Red Card",
        "Green Card",
        "Purple Card",
    };

    [SerializeField] private float[] colorProbabilities = { 0.5f, 0.12f, 0.12f, 0.12f, 0.12f, 0.01f, 0.01f };

    // List of rare cards
    private List<string> rareCards = new List<string>()
    {
        "Green Card",
        "Purple Card",
        // Add other rare cards as needed
    };

    private int numberOfOpenedPacks = 0;
    private int numberOfRareCards = 0;

    // Add a CustomMask class to include the destruction state
    [System.Serializable]
    public class CustomMask
    {
        public RawImage mask;
        public bool destroyed;
    }

    // Declare a dictionary to map the card name to the corresponding CustomMask
    private Dictionary<string, CustomMask> cardToMask = new Dictionary<string, CustomMask>();

    void Start()
    {
        // Add each card and its CustomMask to the dictionary
        cardToMask.Add("Blue Card", new CustomMask { mask = Mask1 });
        cardToMask.Add("Yellow Card", new CustomMask { mask = Mask2 });
        cardToMask.Add("Orange Card", new CustomMask { mask = Mask3 });
        cardToMask.Add("Pink Card", new CustomMask { mask = Mask4 });
        cardToMask.Add("Red Card", new CustomMask { mask = Mask5 });
        cardToMask.Add("Green Card", new CustomMask { mask = Mask6 });
        cardToMask.Add("Purple Card", new CustomMask { mask = Mask7 });
    }

    public TextMeshProUGUI cardsInfoText;

    public void ShowPossibleCards()
    {
        string cardsInfo = "Possible cards:\n";

        foreach (string card in cards)
        {
            cardsInfo += card + "\n";
        }

        if (cardsInfoText != null)
        {
            cardsInfoText.text = cardsInfo;
        }
        else
        {
            Debug.LogError("UI Text not assigned to display card information.");
        }
    }

    public void HidePossibleCards()
    {
        if (cardsInfoText != null)
        {
            cardsInfoText.text = "";
        }
        else
        {
            Debug.LogError("UI Text not assigned to display card information.");
        }
    }

    public void GetCards()
    {
        if (tokens >= 5f)
        {
            DestroyExistingCards();

            tokens -= 5f;

            List<string> obtainedCards = GetRandomCards(3);

            numberOfOpenedPacks++;

            DisplayCardsAndInfoInUI(obtainedCards);

            CreateCardObjects(obtainedCards);

            CheckRareCards(obtainedCards);
        }
        else
        {
            Debug.Log("Not enough tokens to get cards.");
        }
    }

    private List<string> GetRandomCards(int numberOfCards)
    {
        List<string> randomCards = new List<string>();

        System.Random random = new System.Random();

        for (int i = 0; i < numberOfCards; i++)
        {
            float randomValue = (float)random.NextDouble();

            float cumulativeProbability = 0f;

            for (int j = 0; j < cards.Count; j++)
            {
                cumulativeProbability += colorProbabilities[j];

                if (randomValue <= cumulativeProbability)
                {
                    randomCards.Add(cards[j]);
                    break;
                }
            }
        }

        return randomCards;
    }

    private void DisplayCardsAndInfoInUI(List<string> obtainedCards)
    {
        if (consoleText != null)
        {
            string newText = "Obtained cards: ";

            foreach (string card in obtainedCards)
            {
                newText += "\n" + card;
            }

            newText += "\n\nOpened packs: " + numberOfOpenedPacks;
            newText += "\n\nRemaining tokens: " + tokens;

            consoleText.text = newText;
        }
        else
        {
            Debug.LogError("UI Text not assigned to display cards and information.");
        }
    }

    private void CheckRareCards(List<string> obtainedCards)
    {
        foreach (string card in obtainedCards)
        {
            if (rareCards.Contains(card))
            {
                numberOfRareCards++;
            }
        }

        if (rareCardsText != null)
        {
            rareCardsText.text = "Rare cards obtained: " + numberOfRareCards;
        }
        else
        {
            Debug.LogError("UI Text not assigned to display the number of rare cards.");
        }
    }

    private void CreateCardObjects(List<string> obtainedCards)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }

        float distanceBetweenCards = 2.5f;

        Vector3 lastPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceSpawnInFrontOfCamera;

        foreach (string card in obtainedCards)
        {
            int cardIndex = cards.IndexOf(card);

            if (cardIndex >= 0 && cardIndex < cardsPrefabs.Length)
            {
                Vector3 positionInFrontOfCamera = lastPosition + Vector3.right * distanceBetweenCards;

                GameObject newCard = Instantiate(cardsPrefabs[cardIndex], positionInFrontOfCamera, Quaternion.identity);

                lastPosition = positionInFrontOfCamera;

                // Check if the drawn card is in the dictionary and if the corresponding mask has not been destroyed
                if (cardToMask.ContainsKey(card) && !cardToMask[card].destroyed)
                {
                    // Destroy the mask associated with the card
                    Destroy(cardToMask[card].mask.gameObject);

                    // Mark the mask as destroyed to avoid future destruction
                    cardToMask[card].destroyed = true;
                }
            }
            else
            {
                Debug.LogError("Invalid card index: " + cardIndex);
            }
        }
    }

    private void DestroyExistingCards()
    {
        GameObject[] existingCards = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject existingCard in existingCards)
        {
            Destroy(existingCard);
        }
    }
}
