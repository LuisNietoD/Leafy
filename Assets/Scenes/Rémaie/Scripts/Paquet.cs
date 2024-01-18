using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

[ExecuteInEditMode]
public class CartesManager : MonoBehaviour
{
    public float jetons = 50; // Le nombre de jetons que le joueur poss�de
    public TextMeshProUGUI consoleText; // R�f�rence au Text UI pour afficher les r�sultats dans la console
    public TextMeshProUGUI cartesRaresText; // Text UI pour afficher le nombre de cartes rares obtenues
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

    // Tableau de probabilit�s associ� � chaque couleur de carte
    [SerializeField] private float[] probabilitesCouleurs = { 0.5f, 0.12f, 0.12f, 0.12f, 0.12f, 0.06f, 0.06f }; // Modifiez ces valeurs selon vos besoins

    [SerializeField]
    [Range(0.0f, 1.0f)] // D�finir la plage de valeurs autoris�es entre 0.0 et 1.0
    private float seuilRarete = 0.06f; // Valeur qui d�finit qu'une carte est rare

    private int nombrePaquetsOuverts = 0; // Variable pour stocker le nombre de paquets ouverts
    private int nombreCartesRares = 0; // Variable pour stocker le nombre de cartes rares obtenues

    // Fonction appel�e lorsque le joueur appuie sur le bouton


    public TextMeshProUGUI infoCartesText; // R�f�rence au Text UI pour afficher les informations sur les cartes

    // Fonction appel�e lorsque la souris entre dans la zone du bouton
    public void AfficherCartesPossibles()
    {
        string infoCartes = "Cartes possibles :\n";

        foreach (string carte in cartes)
        {
            infoCartes += carte + "\n";
        }

        // Affiche les informations sur les cartes dans le Text UI d�di�
        if (infoCartesText != null)
        {
            infoCartesText.text = infoCartes;
        }
        else
        {
            Debug.LogError("Text UI non assign� pour afficher les informations sur les cartes.");
        }
    }

    // Fonction appel�e lorsque la souris quitte la zone du bouton
    public void CacherCartesPossibles()
    {
        // Efface le texte d'information sur les cartes
        if (infoCartesText != null)
        {
            infoCartesText.text = "";
        }
        else
        {
            Debug.LogError("Text UI non assign� pour afficher les informations sur les cartes.");
        }
    }
    public void ObtenirCartes()
    {
        if (jetons >= 5f) // V�rifie si le joueur a assez de jetons
        {
            // D�truit les cartes existantes
            DetruireCartesExistants();

            jetons -= 5f; // D�duit le co�t en jetons

            // Obtient 3 cartes al�atoires
            List<string> cartesObtenues = ObtenirCartesAleatoires(3);

            // Incr�mente le nombre de paquets ouverts
            nombrePaquetsOuverts++;

            // Affiche les cartes, le nombre de paquets ouverts et le nombre de jetons dans l'UI
            AfficherCartesEtInfoDansUI(cartesObtenues);

            // Fait appara�tre les objets correspondant aux cartes tir�es
            CreerObjetsCartes(cartesObtenues);

            // V�rifie si des cartes rares ont �t� obtenues et met � jour le compteur
            VerifierCartesRares(cartesObtenues);
        }
        else
        {
            Debug.Log("Pas assez de jetons pour obtenir des cartes.");
        }
    }

    // Fonction pour obtenir un nombre donn� de cartes al�atoires en fonction des probabilit�s
    private List<string> ObtenirCartesAleatoires(int nombreDeCartes)
    {
        List<string> cartesAleatoires = new List<string>();

        System.Random random = new System.Random();

        for (int i = 0; i < nombreDeCartes; i++)
        {
            float randomValue = (float)random.NextDouble(); // G�n�re une valeur al�atoire entre 0 et 1

            float cumulativeProbability = 0f;

            // S�lectionne une carte en fonction des probabilit�s
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

            // Ajoute le nombre actuel de paquets ouverts au Text UI
            nouveauTexte += "\n\nPaquets ouverts : " + nombrePaquetsOuverts;

            // Ajoute le nombre actuel de jetons au Text UI
            nouveauTexte += "\n\nJetons restants : " + jetons;

            // R�assigne le texte
            consoleText.text = nouveauTexte;
        }
        else
        {
            Debug.LogError("Text UI non assign� pour afficher les cartes et les informations.");
        }
    }

    // Fonction pour v�rifier les cartes rares et mettre � jour le compteur
    private void VerifierCartesRares(List<string> cartesObtenues)
    {
        // V�rifie s'il y a des cartes rares dans la liste obtenue
        foreach (string carte in cartesObtenues)
        {
            if (probabilitesCouleurs[cartes.IndexOf(carte)] == seuilRarete)
            {
                // Incr�mente le nombre de cartes rares
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
            Debug.LogError("Text UI non assign� pour afficher le nombre de cartes rares.");
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

        // M�morise la position de la derni�re carte instanci�e
        Vector3 dernierePosition = mainCamera.transform.position + mainCamera.transform.forward * distanceSpawnDevantCamera;

        foreach (string carte in cartesObtenues)
        {
            // Trouve l'index de la carte dans la liste originale
            int indexCarte = cartes.IndexOf(carte);

            // Si l'index est valide dans le tableau de pr�fabs
            if (indexCarte >= 0 && indexCarte < prefabsCartes.Length)
            {
                // Calcule la position devant la cam�ra avec un espace entre les cartes et un d�calage en fonction de la derni�re position
                Vector3 positionDevantCamera = dernierePosition + Vector3.right * distanceEntreCartes;

                // Instancie un nouvel objet � partir du pr�fab associ� � la carte
                GameObject nouvelleCarte = Instantiate(prefabsCartes[indexCarte], positionDevantCamera, Quaternion.identity);

                // Mise � jour de la derni�re position
                dernierePosition = positionDevantCamera;

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
