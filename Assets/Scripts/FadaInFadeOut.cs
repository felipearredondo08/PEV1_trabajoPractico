using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleFade : MonoBehaviour
{
    public Image fadeImage; // Asigna aquí la imagen negra en el Inspector
    public float fadeInDuration = 2f; // Duración del fade in (en segundos)
    public float fadeOutDuration = 2f; // Duración del fade out (en segundos)
    public float delayBeforeFadeIn = 3f; // Retraso antes del fade in (en segundos)
    public float delayBeforeFadeOut = 1f; // Retraso antes del fade out (en segundos)

    private void Start()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Fade in con retraso
        yield return new WaitForSeconds(delayBeforeFadeIn);
        yield return StartCoroutine(FadeIn());

        // Retraso antes del fade out
        yield return new WaitForSeconds(delayBeforeFadeOut);
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f; // Comienza totalmente opaco
        fadeImage.color = color;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeInDuration); // Interpola de 1 a 0
            fadeImage.color = color;
            yield return null; // Espera al siguiente frame
        }

        // Asegúrate de que sea completamente transparente al final
        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f; // Comienza totalmente transparente
        fadeImage.color = color;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeOutDuration); // Interpola de 0 a 1
            fadeImage.color = color;
            yield return null; // Espera al siguiente frame
        }

        // Asegúrate de que sea completamente opaco al final
        color.a = 1f;
        fadeImage.color = color;
    }
}