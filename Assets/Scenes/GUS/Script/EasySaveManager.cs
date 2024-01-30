using System.Collections.Generic;
using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes.GUS.Script
{
    public class EasySaveManager : MonoBehaviour
    {
        public Transform[] gameObjectsToSave;
        private string saveKey = "objectPositions";

        private void Start()
        {
            // Vérifier s'il existe un fichier de sauvegarde et le charger si c'est le cas
            if (ES3.KeyExists(saveKey))
            {
                LoadObjectPositions();
            }

            // Commencez la sauvegarde automatique toutes les 3 secondes
            StartCoroutine(AutoSaveRoutine());
        }

        private IEnumerator AutoSaveRoutine()
        {
            while (true)
            {
                // Attendre 3 secondes
                yield return new WaitForSeconds(3f);

                // Appeler la méthode de sauvegarde
                SaveObjectPositions();
            }
        }

        private void SaveObjectPositions()
        {
            Debug.Log("Sauvegarde des positions...");
            // Créer une liste pour stocker les positions
            List<Vector3> positions = new List<Vector3>();

            // Enregistrer les positions des gameObjectsToSave dans la liste
            foreach (Transform objTransform in gameObjectsToSave)
            {
                positions.Add(objTransform.position);
            }

            // Enregistrer la liste dans Easy Save 3
            ES3.Save(saveKey, positions.ToArray());
        }

        // Méthode pour charger les positions
        private void LoadObjectPositions()
        {
            // Charger la liste depuis Easy Save 3
            object loadedPositionsObj = ES3.Load(saveKey, (Vector3[])null);

            // Vérifier si le chargement est réussi et le type est correct
            if (loadedPositionsObj is Vector3[])
            {
                Vector3[] loadedPositions = (Vector3[])loadedPositionsObj;

                // Appliquer les positions aux gameObjectsToSave
                for (int i = 0; i < Mathf.Min(loadedPositions.Length, gameObjectsToSave.Length); i++)
                {
                    gameObjectsToSave[i].position = loadedPositions[i];
                }
            }
            else
            {
                Debug.LogWarning("Chargement des positions échoué ou le type est incorrect.");
            }
        }
    }
}
