using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup; // Asigna el CanvasGroup desde el Inspector
    public float fadeDuration = 2f;  // Duración total del fade
    public float maxAlpha = 1f;      // Opacidad máxima (negro total)

    private void Start()
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("⚠️ Falta asignar el CanvasGroup al FadeController.");
            return;
        }

        // Asegurar que el CanvasGroup comienza en negro
        fadeCanvasGroup.alpha = maxAlpha;
        fadeCanvasGroup.blocksRaycasts = true; // Bloquear interacción durante el fade

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            fadeCanvasGroup.alpha = Mathf.SmoothStep(maxAlpha, 0f, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f; // Asegurar que el fade termina completamente
        fadeCanvasGroup.blocksRaycasts = false; // Permitir interacción después del fade
    }
}
