using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameObject prefabToSpawn; // R�f�rence au prefab � cr�er

    private void OnTriggerEnter(Collider other)
    {
        // V�rifie si l'objet en collision a le tag "Sell"
        if (other.CompareTag("Sell"))
        {
            Debug.Log("Aled");
            // D�truit le prefab actuel
            Destroy(gameObject);

            // Cr�e deux nouveaux prefabs
            SpawnPrefabs();
        }
    }

    private void SpawnPrefabs()
    {
        // Cr�e deux nouveaux prefabs � la position actuelle
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
