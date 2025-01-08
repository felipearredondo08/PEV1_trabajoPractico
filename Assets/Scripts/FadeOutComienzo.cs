using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutComienzo : MonoBehaviour
{
    public Image fadeImage; // Imagen negra en el canvas
    public float fadeDuration = 1f; // Duración del fade out
    public float delayBeforeFade = 1f; // Tiempo antes de comenzar el desvanecido

    private bool isFading = false;

    private void Start()
    {
        if (fadeImage != null)
        {
            // Asegurarse de que la imagen comienza completamente opaca (oscura)
            Color color = fadeImage.color;
            color.a = 1; // Comienza completamente opaco
            fadeImage.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Personaje" && !isFading)
        {
            StartCoroutine(FadeOutWithDelay());
        }
    }

    private IEnumerator FadeOutWithDelay()
    {
        isFading = true;

        // Esperar el tiempo definido antes de iniciar el desvanecido
        yield return new WaitForSeconds(delayBeforeFade);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / fadeDuration); // De opaco (oscuro) a transparente (claro)

            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha;
                fadeImage.color = color;
            }

            yield return null;
        }

        // Asegurarse de que la imagen sea completamente transparente al final
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0;
            fadeImage.color = color;
        }

        isFading = false;
    }
}