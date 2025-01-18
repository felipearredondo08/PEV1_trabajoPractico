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

    [Header("Player Settings")]
    public GameObject player; // Referencia al jugador
    public Sprite backFacingSprite; // Sprite del jugador de espaldas

    private bool isPlayerInTrigger = false;
    private bool isTransitioning = false;
    private float originalOrthographicSize; // Tamaño original de la cámara
    private SpriteRenderer playerSpriteRenderer; // Componente SpriteRenderer del jugador
    private Animator playerAnimator; // Componente Animator del jugador (si existe)

    private void Start()
    {
        if (virtualCamera != null)
        {
            originalOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }

        if (player != null)
        {
            playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            playerAnimator = player.GetComponent<Animator>(); // Obtiene el Animator (si existe)
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
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.UpArrow) && !isTransitioning)
        {
            StartCoroutine(FadeZoomAndLoadLevel());
        }
    }

    private IEnumerator FadeZoomAndLoadLevel()
    {
        isTransitioning = true;

        // Cambia el sprite del jugador
        if (playerSpriteRenderer != null && backFacingSprite != null)
        {
            if (playerAnimator != null)
            {
                playerAnimator.enabled = false; // Desactiva el Animator para evitar conflictos
            }
            playerSpriteRenderer.sprite = backFacingSprite;
        }

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

            virtualCamera.m_Lens.OrthographicSize = zoomTarget;
        }
    }
}