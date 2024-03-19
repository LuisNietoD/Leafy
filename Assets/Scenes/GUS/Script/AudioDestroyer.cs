using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioDestroyer : MonoBehaviour
{
    public string targetSceneName = "MyScene"; // Nom de la scène où l'audio source doit être détruit

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // S'abonner à l'événement de changement de scène
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Se désabonner de l'événement de changement de scène pour éviter les fuites de mémoire
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Vérifier si la scène chargée est la scène cible
        if (scene.name == targetSceneName)
        {
            // Si c'est la scène cible, détruire l'audio source
            Destroy(audioSource.gameObject);
        }
    }
}