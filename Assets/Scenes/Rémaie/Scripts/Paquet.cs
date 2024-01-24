using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

[ExecuteInEditMode]
public class CartesManager : MonoBehaviour
{
    public float jetons = 50;
    public TextMeshProUGUI consoleText;
    public TextMeshProUGUI cartesRaresText;
    public GameObject[] prefabsCartes;
    public float distanceSpawnDevantCamera = 5f;
    public RawImage Mask1;
    public RawImage Mask2;
    public RawImage Mask3;
    public RawImage Mask4;
    public RawImage Mask5;
    public RawImage Mask6;
    public RawImage Mask7;

    // Liste de cartes différentes
    private List<string> cartes = new List<string>()
    {
        "Carte Bleu",
        "Carte Jaune",
        "Carte Orange",
        "Carte Rose",
        "Carte Rouge",
        "Carte Vert",
        "Carte Violette",
    };

    [SerializeField] private float[] probabilitesCouleurs = { 0.5f, 0.12f, 0.12f, 0.12f, 0.12f, 0.01f, 0.01f };

    [SerializeField][Range(0.0f, 1.0f)] private float seuilRarete = 0.02f;

    // Liste de cartes rares
    private List<string> cartesRares = new List<string>()
    {
        "Carte Vert",
        "Carte Violette",
        // Ajoutez d'autres cartes rares si nécessaire
    };

    private int nombrePaquetsOuverts = 0;
    private int nombreCartesRares = 0;

    // Ajoute une classe CustomMask pour inclure l'état de destruction
    [System.Serializable]
    public class CustomMask
    {
        public RawImage mask;
        public bool destroyed;
    }

    // Déclare un dictionnaire pour mapper le nom de la carte à la CustomMask correspondante
    private Dictionary<string, CustomMask> carteToMask = new Dictionary<string, CustomMask>();

    void Start()
    {
        // Ajoute chaque carte et sa CustomMask au dictionnaire
        carteToMask.Add("Carte Bleu", new CustomMask { mask = Mask1 });
        carteToMask.Add("Carte Jaune", new CustomMask { mask = Mask2 });
        carteToMask.Add("Carte Orange", new CustomMask { mask = Mask3 });
        carteToMask.Add("Carte Rose", new CustomMask { mask = Mask4 });
        carteToMask.Add("Carte Rouge", new CustomMask { mask = Mask5 });
        carteToMask.Add("Carte Vert", new CustomMask { mask = Mask6 });
        carteToMask.Add("Carte Violette", new CustomMask { mask = Mask7 });
    }

    public TextMeshProUGUI infoCartesText;

    public void AfficherCartesPossibles()
    {
        string infoCartes = "Cartes possibles :\n";

        foreach (string carte in cartes)
        {
            infoCartes += carte + "\n";
        }

        if (infoCartesText != null)
        {
            infoCartesText.text = infoCartes;
        }
        else
        {
            Debug.LogError("Text UI non assigné pour afficher les informations sur les cartes.");
        }
    }

    public void CacherCartesPossibles()
    {
        if (infoCartesText != null)
        {
            infoCartesText.text = "";
        }
        else
        {
            Debug.LogError("Text UI non assigné pour afficher les informations sur les cartes.");
        }
    }

    public void ObtenirCartes()
    {
        if (jetons >= 5f)
        {
            DetruireCartesExistants();

            jetons -= 5f;

            List<string> cartesObtenues = ObtenirCartesAleatoires(3);

            nombrePaquetsOuverts++;

            AfficherCartesEtInfoDansUI(cartesObtenues);

            CreerObjetsCartes(cartesObtenues);

            VerifierCartesRares(cartesObtenues);
        }
        else
        {
            Debug.Log("Pas assez de jetons pour obtenir des cartes.");
        }
    }

    private List<string> ObtenirCartesAleatoires(int nombreDeCartes)
    {
        List<string> cartesAleatoires = new List<string>();

        System.Random random = new System.Random();

        for (int i = 0; i < nombreDeCartes; i++)
        {
            float randomValue = (float)random.NextDouble();

            float cumulativeProbability = 0f;

            for (int j = 0; j < cartes.Count; j++)
            {
                cumulativeProbability += probabilitesCouleurs[j];

                if (randomValue <= cumulativeProbability)
                {
                    cartesAleatoires.Add(cartes[j]);
                    break;
                }
            }
        }

        return cartesAleatoires;
    }

    private void AfficherCartesEtInfoDansUI(List<string> cartesObtenues)
    {
        if (consoleText != null)
        {
            string nouveauTexte = "Cartes obtenues : ";

            foreach (string carte in cartesObtenues)
            {
                nouveauTexte += "\n" + carte;
            }

            nouveauTexte += "\n\nPaquets ouverts : " + nombrePaquetsOuverts;
            nouveauTexte += "\n\nJetons restants : " + jetons;

            consoleText.text = nouveauTexte;
        }
        else
        {
            Debug.LogError("Text UI non assigné pour afficher les cartes et les informations.");
        }
    }

    private void VerifierCartesRares(List<string> cartesObtenues)
    {
        foreach (string carte in cartesObtenues)
        {
            if (cartesRares.Contains(carte))
            {
                nombreCartesRares++;
            }
        }

        if (cartesRaresText != null)
        {
            cartesRaresText.text = "Cartes rares obtenues : " + nombreCartesRares;
        }
        else
        {
            Debug.LogError("Text UI non assigné pour afficher le nombre de cartes rares.");
        }
    }

    private void CreerObjetsCartes(List<string> cartesObtenues)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("La caméra principale n'est pas trouvée.");
            return;
        }

        float distanceEntreCartes = 2.5f;

        Vector3 dernierePosition = mainCamera.transform.position + mainCamera.transform.forward * distanceSpawnDevantCamera;

        foreach (string carte in cartesObtenues)
        {
            int indexCarte = cartes.IndexOf(carte);

            if (indexCarte >= 0 && indexCarte < prefabsCartes.Length)
            {
                Vector3 positionDevantCamera = dernierePosition + Vector3.right * distanceEntreCartes;

                GameObject nouvelleCarte = Instantiate(prefabsCartes[indexCarte], positionDevantCamera, Quaternion.identity);

                dernierePosition = positionDevantCamera;

                // Vérifie si la carte tirée est dans le dictionnaire et si le masque correspondant n'a pas été détruit
                if (carteToMask.ContainsKey(carte) && !carteToMask[carte].destroyed)
                {
                    // Détruit le masque associé à la carte
                    Destroy(carteToMask[carte].mask.gameObject);

                    // Marque le masque comme détruit pour éviter une destruction ultérieure
                    carteToMask[carte].destroyed = true;
                }
            }
            else
            {
                Debug.LogError("Index de carte invalide : " + indexCarte);
            }
        }
    }

    private void DetruireCartesExistants()
    {
        GameObject[] cartesExistantes = GameObject.FindGameObjectsWithTag("Carte");

        foreach (GameObject carteExistante in cartesExistantes)
        {
            Destroy(carteExistante);
        }
    }
}
