using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Necesario para IEnumerator

public class SimpleFadeIn : MonoBehaviour
{
    public Image fadeImage; // Asigna aquí la imagen negra en el Inspector
    public float fadeDuration = 2f; // Duración del fade in (en segundos)
    public float delayBeforeFade = 3f; // Retraso antes de iniciar el fade in (en segundos)

    private void Start()
    {
        StartCoroutine(DelayedFadeIn());
    }

    private IEnumerator DelayedFadeIn()
    {
        // Asegúrate de que la imagen esté completamente negra al inicio
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        // Espera el tiempo de retraso antes de comenzar el fade in
        yield return new WaitForSeconds(delayBeforeFade);

        // Inicia el fade in
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Interpola de 1 a 0
            fadeImage.color = color;
            yield return null; // Espera al siguiente frame
        }

        // Asegúrate de que el alpha sea exactamente 0 al final
        color.a = 0f;
        fadeImage.color = color;
    }
}