/*using UnityEngine;
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
}*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LoadSceneWithFade : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Nivel1"; // Nombre de la escena a cargar
    [SerializeField] private float fadeInDuration = 1.5f; // Duración del fade in
    [SerializeField] private float delayBeforeFadeIn = 1f; // Tiempo antes del fade in
    [SerializeField] private float delayBeforeFadeOut = 3f; // Tiempo de espera antes del fade out
    [SerializeField] private float fadeOutDuration = 1.5f; // Duración del fade out
    [SerializeField] private float delayAfterFadeOut = 0.5f; // Tiempo de espera después del fade out
    [SerializeField] private CanvasGroup canvasGroup; // Se debe asignar en el Inspector

    private void Start()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("No se ha asignado el CanvasGroup en el Inspector.");
            return;
        }

        // Empezar con el Canvas invisible
        canvasGroup.alpha = 0f;
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Esperar antes del fade in
        yield return new WaitForSeconds(fadeInDuration);

        // Fade in del Canvas y sus hijos
        yield return StartCoroutine(Fade(0f, 1f, fadeInDuration));

        // Esperar el tiempo de permanencia
        yield return new WaitForSeconds(delayBeforeFadeOut);

        // Fade out del Canvas y sus hijos
        yield return StartCoroutine(Fade(1f, 0f, fadeOutDuration));

        // Esperar después del fade out antes de cambiar de escena
        yield return new WaitForSeconds(delayAfterFadeOut);

        // Cargar la siguiente escena
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha; // Asegurar que el alpha termina exactamente en el valor esperado
    }
}
