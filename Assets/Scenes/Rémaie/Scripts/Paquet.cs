using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class CartesManager : MonoBehaviour
{
    public float jetons = 50; // Le nombre de jetons que le joueur possède
    public TextMeshProUGUI consoleText; // Référence au Text UI pour afficher les résultats dans la console
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

            // Affiche les cartes et le nombre de jetons dans l'UI
            AfficherCartesEtJetonsDansUI(cartesObtenues);

            // Fait apparaître les objets correspondant aux cartes tirées
            CreerObjetsCartes(cartesObtenues);
        }
        else
        {
            Debug.Log("Pas assez de jetons pour obtenir des cartes.");
        }
    }

    // Fonction pour obtenir un nombre donné de cartes aléatoires depuis la liste
    private List<string> ObtenirCartesAleatoires(int nombreDeCartes)
    {
        List<string> cartesAleatoires = new List<string>();
        List<string> cartesDisponibles = new List<string>(cartes);

        System.Random random = new System.Random();

        for (int i = 0; i < nombreDeCartes; i++)
        {
            int indexCarte = random.Next(cartesDisponibles.Count);
            cartesAleatoires.Add(cartesDisponibles[indexCarte]);
            cartesDisponibles.RemoveAt(indexCarte);
        }

        return cartesAleatoires;
    }

    // Fonction pour afficher les cartes et le nombre de jetons dans l'UI
    private void AfficherCartesEtJetonsDansUI(List<string> cartesObtenues)
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

            // Ajoute le nombre actuel de jetons au Text UI
            nouveauTexte += "\n\nJetons restants : " + jetons;

            // Réassigne le texte
            consoleText.text = nouveauTexte;
        }
        else
        {
            Debug.LogError("Text UI non assigné pour afficher les cartes et les jetons.");
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

        foreach (string carte in cartesObtenues)
        {
            // Trouve l'index de la carte dans la liste originale
            int indexCarte = cartes.IndexOf(carte);

            // Si l'index est valide dans le tableau de préfabs
            if (indexCarte >= 0 && indexCarte < prefabsCartes.Length)
            {
                // Calcule la position devant la caméra avec un espace entre les cartes
                Vector3 positionDevantCamera = mainCamera.transform.position + mainCamera.transform.forward * distanceSpawnDevantCamera + Vector3.right * distanceEntreCartes * cartesObtenues.IndexOf(carte);

                // Instancie un nouvel objet à partir du préfab associé à la carte
                GameObject nouvelleCarte = Instantiate(prefabsCartes[indexCarte], positionDevantCamera, Quaternion.identity);

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
