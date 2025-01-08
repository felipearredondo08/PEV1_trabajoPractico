
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    public Image fadeImage; // Imagen negra en el canvas
    public float fadeDuration = 1f; // Duración del fade
    public string nextLevel = "nivel2"; // Nombre del nivel al que se cambiará

    private bool isPlayerInTrigger = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(FadeAndLoadLevel());
        }
    }

    private IEnumerator FadeAndLoadLevel()
    {
        // Inicia el fade out
        if (fadeImage != null)
        {
            fadeImage.color = Color.black;
            fadeImage.gameObject.SetActive(true);
            fadeImage.canvasRenderer.SetAlpha(0f); // Comienza transparente
            fadeImage.CrossFadeAlpha(1f, fadeDuration, false); // Fade hacia opaco
        }

        // Espera el tiempo del fade
        yield return new WaitForSeconds(fadeDuration);

        // Cambia de escena
        SceneManager.LoadScene(nextLevel);
    }
}