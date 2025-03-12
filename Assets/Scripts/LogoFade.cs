using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoFade : MonoBehaviour
{
    [SerializeField] private Image logoImage;
    [SerializeField] private AudioSource audioSource; // Sonido al iniciar el fade in
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float fadeInSpeed = 1f;
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private float fadeOutSpeed = 1f;
    [SerializeField] private float delayBeforeNextScene = 1f;
    [SerializeField] private string nextSceneName = "MainMenu";

    private void Start()
    {
        StartCoroutine(FadeLogoRoutine());
    }

    private IEnumerator FadeLogoRoutine()
    {
        // Asegurar que el logo empieza invisible
        SetAlpha(0);

        // Esperar antes del fade in
        yield return new WaitForSeconds(startDelay);

        // Reproducir sonido cuando inicia el fade in
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Fade In
        yield return StartCoroutine(Fade(0f, 1f, fadeInSpeed));

        // Mantener en pantalla
        yield return new WaitForSeconds(displayTime);

        // Fade Out
        yield return StartCoroutine(Fade(1f, 0f, fadeOutSpeed));

        // Esperar antes de cambiar de escena
        yield return new WaitForSeconds(delayBeforeNextScene);

        // Cargar siguiente escena
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = logoImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.SmoothStep(startAlpha, targetAlpha, elapsedTime / duration);
            SetAlpha(alpha);
            yield return null;
        }

        // Asegurar que la opacidad final sea exacta
        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = logoImage.color;
        color.a = alpha;
        logoImage.color = color;
    }
}

