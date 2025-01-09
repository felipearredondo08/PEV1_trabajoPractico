using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Cinemachine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage; // Imagen negra en el canvas
    public float fadeDuration = 1f; // Duración del fade
    public string nextLevel = "nivel2"; // Nombre del nivel al que se cambiará

    [Header("Camera Zoom Settings")]
    public CinemachineVirtualCamera virtualCamera; // Referencia a la cámara de Cinemachine
    public float zoomDuration = 1f; // Duración del zoom
    public float zoomTarget = 3f; // Tamaño del zoom

    private bool isPlayerInTrigger = false;
    private float originalOrthographicSize; // Tamaño original de la cámara
    private bool isTransitioning = false;

    private void Start()
    {
        if (virtualCamera != null)
        {
            originalOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
        }

        if (fadeImage != null)
        {
            // Asegurarse de que el fade image comienza desactivado
            fadeImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && !isTransitioning)
        {
            StartCoroutine(FadeAndZoomAndLoadLevel());
        }
    }

    private IEnumerator FadeAndZoomAndLoadLevel()
    {
        isTransitioning = true;

        // Inicia el zoom
        if (virtualCamera != null)
        {
            StartCoroutine(ZoomIn());
        }

        // Inicia el fade in
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = Color.black;
            fadeImage.canvasRenderer.SetAlpha(0f); // Comienza transparente
            fadeImage.CrossFadeAlpha(1f, fadeDuration, false); // Fade hacia opaco
        }

        // Espera el tiempo del fade
        yield return new WaitForSeconds(fadeDuration);

        // Cambia de escena
        SceneManager.LoadScene(nextLevel);
    }

    private IEnumerator ZoomIn()
    {
        float timer = 0f;

        if (virtualCamera != null)
        {
            float initialSize = virtualCamera.m_Lens.OrthographicSize;

            while (timer < zoomDuration)
            {
                timer += Time.deltaTime;
                float t = timer / zoomDuration;
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(initialSize, zoomTarget, t);
                yield return null;
            }

            // Asegurarse de que el zoom llegue exactamente al objetivo
            virtualCamera.m_Lens.OrthographicSize = zoomTarget;
        }
    }
}