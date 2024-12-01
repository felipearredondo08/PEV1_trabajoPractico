using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Nivel1"; // Nombre de la escena a cargar
    [SerializeField] private float delay = 3f; // Tiempo de espera en segundos

    private void Start()
    {
        // Iniciar la carga de la escena después de 'delay' segundos
        Invoke("LoadScene", delay);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}