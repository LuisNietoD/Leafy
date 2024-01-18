using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class CartesManager : MonoBehaviour
{
    public float jetons = 50; // Le nombre de jetons que le joueur poss�de
    public TextMeshProUGUI consoleText; // R�f�rence au Text UI pour afficher les r�sultats dans la console
    public GameObject[] prefabsCartes; // Tableau de pr�fabs pour chaque carte
    public float distanceSpawnDevantCamera = 5f; // D�finissez la distance � laquelle les objets seront spawn�s devant la cam�ra


    // Liste de cartes diff�rentes
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

    // Fonction appel�e lorsque le joueur appuie sur le bouton
    public void ObtenirCartes()
    {
        if (jetons >= 5f) // V�rifie si le joueur a assez de jetons
        {
            // D�truit les cartes existantes
            DetruireCartesExistants();

            jetons -= 5f; // D�duit le co�t en jetons

            // Obtient 3 cartes al�atoires
            List<string> cartesObtenues = ObtenirCartesAleatoires(3);

            // Affiche les cartes et le nombre de jetons dans l'UI
            AfficherCartesEtJetonsDansUI(cartesObtenues);

            // Fait appara�tre les objets correspondant aux cartes tir�es
            CreerObjetsCartes(cartesObtenues);
        }
        else
        {
            Debug.Log("Pas assez de jetons pour obtenir des cartes.");
        }
    }

    // Fonction pour obtenir un nombre donn� de cartes al�atoires depuis la liste
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
        // V�rifie s'il y a un Text UI assign�
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

            // R�assigne le texte
            consoleText.text = nouveauTexte;
        }
        else
        {
            Debug.LogError("Text UI non assign� pour afficher les cartes et les jetons.");
        }
    }

    // Fonction pour cr�er des objets correspondant aux cartes tir�es
    private void CreerObjetsCartes(List<string> cartesObtenues)
    {
        // V�rifie si la cam�ra principale est disponible
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("La cam�ra principale n'est pas trouv�e.");
            return;
        }

        // D�finir la distance entre les cartes
        float distanceEntreCartes = 2.5f; // Vous pouvez ajuster cette valeur en fonction de votre besoin

        foreach (string carte in cartesObtenues)
        {
            // Trouve l'index de la carte dans la liste originale
            int indexCarte = cartes.IndexOf(carte);

            // Si l'index est valide dans le tableau de pr�fabs
            if (indexCarte >= 0 && indexCarte < prefabsCartes.Length)
            {
                // Calcule la position devant la cam�ra avec un espace entre les cartes
                Vector3 positionDevantCamera = mainCamera.transform.position + mainCamera.transform.forward * distanceSpawnDevantCamera + Vector3.right * distanceEntreCartes * cartesObtenues.IndexOf(carte);

                // Instancie un nouvel objet � partir du pr�fab associ� � la carte
                GameObject nouvelleCarte = Instantiate(prefabsCartes[indexCarte], positionDevantCamera, Quaternion.identity);

                // Vous pouvez personnaliser la position, l'orientation, ou d'autres propri�t�s de l'objet ici
            }
            else
            {
                Debug.LogError("Index de carte invalide : " + indexCarte);
            }
        }
    }

    // Fonction pour d�truire les objets de carte existants
    private void DetruireCartesExistants()
    {
        GameObject[] cartesExistantes = GameObject.FindGameObjectsWithTag("Carte");

        foreach (GameObject carteExistante in cartesExistantes)
        {
            Destroy(carteExistante);
        }
    }
}
