using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public Image fadeImage; // Imagen negra en el canvas
    public float fadeDuration = 1f; // Duración del fade in

    private bool isFading = false;

    private void Start()
    {
        if (fadeImage != null)
        {
            // Asegurarse de que la imagen comienza totalmente transparente
            fadeImage.gameObject.SetActive(true);
            Color color = fadeImage.color;
            color.a = 0;
            fadeImage.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Personaje" && !isFading)
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        isFading = true;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);

            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha;
                fadeImage.color = color;
            }

            yield return null;
        }

        // Asegurarse de que la imagen sea completamente opaca al final
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 1;
            fadeImage.color = color;
        }

        isFading = false;
    }
}