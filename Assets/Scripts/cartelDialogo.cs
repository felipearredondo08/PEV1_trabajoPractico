using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CartelImagen : MonoBehaviour
{
    [SerializeField] private Image cartelImage; // Imagen del cartel (debe tener Color con Alpha)
    [SerializeField] private AudioSource audioSource; // AudioSource para el sonido
    [SerializeField] private AudioClip toggleSound; // Sonido al presionar E
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // Tecla de interacción
    [SerializeField] private float fadeDuration = 0.5f; // Duración del fade
    [SerializeField] private float tiempoMostrando = 10f; // Tiempo antes de ocultarse automáticamente

    private bool isPlayerInRange = false;
    private bool isCartelVisible = false;
    private Coroutine fadeCoroutine;
    private Coroutine hideCoroutine;

    private void Start()
    {
        // Asegurar que el cartel esté invisible al inicio
        if (cartelImage != null)
        {
            Color color = cartelImage.color;
            color.a = 0;
            cartelImage.color = color;
            cartelImage.enabled = false; // Desactiva el renderizado de la imagen
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            if (isCartelVisible)
            {
                OcultarCartel();
            }
            else
            {
                MostrarCartel();
            }

            // Reproducir sonido al presionar E
            if (audioSource != null && toggleSound != null)
            {
                audioSource.PlayOneShot(toggleSound);
            }
        }
    }

    private void MostrarCartel()
    {
        isCartelVisible = true;
        cartelImage.enabled = true; // Activa la imagen antes del fade

        // Cancelar cualquier fade en progreso
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeCanvas(0f, 1f));

        // Iniciar el temporizador para ocultarlo después de X segundos
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(TemporizadorOcultar());
    }

    private void OcultarCartel()
    {
        isCartelVisible = false;

        // Cancelar cualquier fade en progreso
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeCanvas(1f, 0f, true));

        // Detener el temporizador si el jugador cierra el cartel antes
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }
    }

    private IEnumerator TemporizadorOcultar()
    {
        yield return new WaitForSeconds(tiempoMostrando);
        OcultarCartel();
    }

    private IEnumerator FadeCanvas(float startAlpha, float targetAlpha, bool disableAfter = false)
    {
        float elapsedTime = 0f;
        Color color = cartelImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            cartelImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        cartelImage.color = color;

        if (disableAfter)
        {
            cartelImage.enabled = false; // Desactiva la imagen después del fade out
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            OcultarCartel(); // Se oculta si el jugador se aleja
        }
    }
}
