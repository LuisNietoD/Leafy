using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameObject prefabToSpawn; // Référence au prefab à créer

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet en collision a le tag "Sell"
        if (other.CompareTag("Sell"))
        {
            Debug.Log("Aled");
            // Détruit le prefab actuel
            Destroy(gameObject);

            // Crée deux nouveaux prefabs
            SpawnPrefabs();
        }
    }

    private void SpawnPrefabs()
    {
        // Crée deux nouveaux prefabs à la position actuelle
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
