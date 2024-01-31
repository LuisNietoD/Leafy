using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Leafy.Objects;

[ExecuteInEditMode]
public class PackUpgraded : MonoBehaviour
{
    public float tokens = 50;
    public GameObject[] cardsPrefabs;
    public float distanceSpawnInFrontOfCamera = 5f;
    public RawImage Mask1;
    public RawImage Mask2;
    public RawImage Mask3;
    public RawImage Mask4;
    public RawImage Mask5;
    public RawImage Mask6;
    public RawImage Mask7;

    public float distanceBetweenCards = 4.0f; // Distance entre les cartes et le bouton

    public Button firstButton;  // Premier bouton

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

    [System.Serializable]
    public class CustomMask
    {
        public RawImage mask;
        public bool destroyed;
    }

    private Dictionary<string, CustomMask> cardToMask = new Dictionary<string, CustomMask>();

    private int buttonPressCount = 0;

    void Start()
    {
        cardToMask.Add("Blue Card", new CustomMask { mask = Mask1 });
        cardToMask.Add("Yellow Card", new CustomMask { mask = Mask2 });
        cardToMask.Add("Orange Card", new CustomMask { mask = Mask3 });
        cardToMask.Add("Pink Card", new CustomMask { mask = Mask4 });
        cardToMask.Add("Red Card", new CustomMask { mask = Mask5 });
        cardToMask.Add("Green Card", new CustomMask { mask = Mask6 });
        cardToMask.Add("Purple Card", new CustomMask { mask = Mask7 });

        if (firstButton != null)
        {
            firstButton.onClick.AddListener(OnFirstButtonClick);
        }

    }

    private void OnFirstButtonClick()
    {
        buttonPressCount++;

        if (buttonPressCount < 6)
        {
            GetCard(buttonPressCount - 1); // Passer l'indice de la carte à obtenir
        }
        else if (buttonPressCount == 6)
        {
            // Après la 5e pression, désactive le premier bouton et active le deuxième bouton
            firstButton.gameObject.SetActive(false);
            buttonPressCount = 0; // Réinitialise le compteur de pression de bouton
            DestroyExistingCards(); // Détruit toutes les cartes existantes

        }
    }


    private void GetCard(int cardIndex)
    {
        if (tokens >= 5f)
        {
            tokens -= 5f;

            List<string> obtainedCards = GetRandomCards(1);

            StartCoroutine(ShowCardsOneByOne(obtainedCards, cardIndex));

        }
        else
        {
            Debug.Log("Not enough tokens to get cards.");
        }
    }

    private void CreateSingleCard(string card, int index)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }

        float horizontalOffset = 2.0f;
        float verticalOffset = 2.0f;

        Vector3 lastPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceSpawnInFrontOfCamera;

        Vector3 positionInFrontOfCamera = lastPosition;

        switch (index)
        {
            case 0: // Top card
                positionInFrontOfCamera += Vector3.up * (verticalOffset + distanceBetweenCards);
                break;
            case 1: // Left card
                positionInFrontOfCamera += Vector3.left * (horizontalOffset + distanceBetweenCards);
                break;
            case 2: // Bottom-left card
                positionInFrontOfCamera += Vector3.down * (verticalOffset + distanceBetweenCards);
                positionInFrontOfCamera += Vector3.left * horizontalOffset;
                break;
            case 3: // Bottom-right card
                positionInFrontOfCamera += Vector3.down * (verticalOffset + distanceBetweenCards);
                positionInFrontOfCamera += Vector3.right * horizontalOffset;
                break;
            case 4: // Right card
                positionInFrontOfCamera += Vector3.right * (horizontalOffset + distanceBetweenCards);
                break;
            default:
                break;
        }

        int cardIndex = cards.IndexOf(card);

        if (cardIndex >= 0 && cardIndex < cardsPrefabs.Length)
        {
            GameObject newCard = Instantiate(cardsPrefabs[cardIndex], positionInFrontOfCamera, Quaternion.identity);
           //newCard.GetComponent<Card>().info (A voir plus tard)

            if (cardToMask.ContainsKey(card) && !cardToMask[card].destroyed)
            {
                Destroy(cardToMask[card].mask.gameObject);
                cardToMask[card].destroyed = true;
            }
        }
        else
        {
            Debug.LogError("Invalid card index: " + cardIndex);
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

    private void DestroyExistingCards()
    {
        GameObject[] existingCards = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject existingCard in existingCards)
        {
            Destroy(existingCard);
        }
    }

    private IEnumerator ShowCardsOneByOne(List<string> obtainedCards, int cardIndex)
    {
        float delayBetweenCards = 0.75f;

        CreateSingleCard(obtainedCards[0], cardIndex);

        // Attendre avant la prochaine carte
        yield return new WaitForSeconds(delayBetweenCards);
    }
}
