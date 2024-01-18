using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

[ExecuteInEditMode]
public class CartesManager : MonoBehaviour
{
    public float jetons = 50; // Le nombre de jetons que le joueur possède
    public TextMeshProUGUI consoleText; // Référence au Text UI pour afficher les résultats dans la console
    public TextMeshProUGUI cartesRaresText; // Text UI pour afficher le nombre de cartes rares obtenues
    public GameObject[] prefabsCartes; // Tableau de préfabs pour chaque carte
    public float distanceSpawnDevantCamera = 5f; // Définissez la distance à laquelle les objets seront spawnés devant la caméra

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

    // Tableau de probabilités associé à chaque couleur de carte
    [SerializeField] private float[] probabilitesCouleurs = { 0.5f, 0.12f, 0.12f, 0.12f, 0.12f, 0.06f, 0.06f }; // Modifiez ces valeurs selon vos besoins

    [SerializeField]
    [Range(0.0f, 1.0f)] // Définir la plage de valeurs autorisées entre 0.0 et 1.0
    private float seuilRarete = 0.06f; // Valeur qui définit qu'une carte est rare

    private int nombrePaquetsOuverts = 0; // Variable pour stocker le nombre de paquets ouverts
    private int nombreCartesRares = 0; // Variable pour stocker le nombre de cartes rares obtenues

    // Fonction appelée lorsque le joueur appuie sur le bouton
    public void ObtenirCartes()
    {
        if (jetons >= 5f) // Vérifie si le joueur a assez de jetons
        {
            // Détruit les cartes existantes
            DetruireCartesExistants();

            jetons -= 5f; // Déduit le coût en jetons

            // Obtient 3 cartes aléatoires
            List<string> cartesObtenues = ObtenirCartesAleatoires(3);

            // Incrémente le nombre de paquets ouverts
            nombrePaquetsOuverts++;

            // Affiche les cartes, le nombre de paquets ouverts et le nombre de jetons dans l'UI
            AfficherCartesEtInfoDansUI(cartesObtenues);

            // Fait apparaître les objets correspondant aux cartes tirées
            CreerObjetsCartes(cartesObtenues);

            // Vérifie si des cartes rares ont été obtenues et met à jour le compteur
            VerifierCartesRares(cartesObtenues);
        }
        else
        {
            Debug.Log("Pas assez de jetons pour obtenir des cartes.");
        }
    }

    // Fonction pour obtenir un nombre donné de cartes aléatoires en fonction des probabilités
    private List<string> ObtenirCartesAleatoires(int nombreDeCartes)
    {
        List<string> cartesAleatoires = new List<string>();

        System.Random random = new System.Random();

        for (int i = 0; i < nombreDeCartes; i++)
        {
            float randomValue = (float)random.NextDouble(); // Génère une valeur aléatoire entre 0 et 1

            float cumulativeProbability = 0f;

            // Sélectionne une carte en fonction des probabilités
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

    // Fonction pour afficher les cartes, le nombre de paquets ouverts et le nombre de jetons dans l'UI
    private void AfficherCartesEtInfoDansUI(List<string> cartesObtenues)
    {
        // Vérifie s'il y a un Text UI assigné
        if (consoleText != null)
        {
            // Efface le texte existant
            string nouveauTexte = "Cartes obtenues : ";

            // Ajoute les cartes obtenues au Text UI
            foreach (string carte in cartesObtenues)
            {
                nouveauTexte += "\n" + carte;
            }

            // Ajoute le nombre actuel de paquets ouverts au Text UI
            nouveauTexte += "\n\nPaquets ouverts : " + nombrePaquetsOuverts;

            // Ajoute le nombre actuel de jetons au Text UI
            nouveauTexte += "\n\nJetons restants : " + jetons;

            // Réassigne le texte
            consoleText.text = nouveauTexte;
        }
        else
        {
            Debug.LogError("Text UI non assigné pour afficher les cartes et les informations.");
        }
    }

    // Fonction pour vérifier les cartes rares et mettre à jour le compteur
    private void VerifierCartesRares(List<string> cartesObtenues)
    {
        // Vérifie s'il y a des cartes rares dans la liste obtenue
        foreach (string carte in cartesObtenues)
        {
            if (probabilitesCouleurs[cartes.IndexOf(carte)] == seuilRarete)
            {
                // Incrémente le nombre de cartes rares
                nombreCartesRares++;
            }
        }

        // Affiche le nombre de cartes rares dans l'UI
        if (cartesRaresText != null)
        {
            cartesRaresText.text = "Cartes rares obtenues : " + nombreCartesRares;
        }
        else
        {
            Debug.LogError("Text UI non assigné pour afficher le nombre de cartes rares.");
        }
    }

    // Fonction pour créer des objets correspondant aux cartes tirées
    private void CreerObjetsCartes(List<string> cartesObtenues)
    {
        // Vérifie si la caméra principale est disponible
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("La caméra principale n'est pas trouvée.");
            return;
        }

        // Définir la distance entre les cartes
        float distanceEntreCartes = 2.5f; // Vous pouvez ajuster cette valeur en fonction de votre besoin

        // Mémorise la position de la dernière carte instanciée
        Vector3 dernierePosition = mainCamera.transform.position + mainCamera.transform.forward * distanceSpawnDevantCamera;

        foreach (string carte in cartesObtenues)
        {
            // Trouve l'index de la carte dans la liste originale
            int indexCarte = cartes.IndexOf(carte);

            // Si l'index est valide dans le tableau de préfabs
            if (indexCarte >= 0 && indexCarte < prefabsCartes.Length)
            {
                // Calcule la position devant la caméra avec un espace entre les cartes et un décalage en fonction de la dernière position
                Vector3 positionDevantCamera = dernierePosition + Vector3.right * distanceEntreCartes;

                // Instancie un nouvel objet à partir du préfab associé à la carte
                GameObject nouvelleCarte = Instantiate(prefabsCartes[indexCarte], positionDevantCamera, Quaternion.identity);

                // Mise à jour de la dernière position
                dernierePosition = positionDevantCamera;

                // Vous pouvez personnaliser la position, l'orientation, ou d'autres propriétés de l'objet ici
            }
            else
            {
                Debug.LogError("Index de carte invalide : " + indexCarte);
            }
        }
    }

    // Fonction pour détruire les objets de carte existants
    private void DetruireCartesExistants()
    {
        GameObject[] cartesExistantes = GameObject.FindGameObjectsWithTag("Carte");

        foreach (GameObject carteExistante in cartesExistantes)
        {
            Destroy(carteExistante);
        }
    }
}
