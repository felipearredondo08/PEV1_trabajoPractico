using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CartelDither : MonoBehaviour
{
    [SerializeField] private Image cartelImage; // Imagen PNG que se muestra
    [SerializeField] private Material ditherMaterial; // Material con el Shader de Dithering
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float tiempoMostrando = 10f;

    private bool isPlayerInRange = false;
    private bool isCartelVisible = false;
    private Coroutine fadeCoroutine;
    private Coroutine hideCoroutine;

    private void Start()
    {
        if (cartelImage != null)
        {
            cartelImage.material = ditherMaterial;
            ditherMaterial.SetFloat("_Cutoff", 1); // Comienza invisible
            cartelImage.enabled = false;
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

            // Sonido al abrir/cerrar
            if (audioSource != null)
            {
                AudioClip clip = isCartelVisible ? openSound : closeSound;
                audioSource.PlayOneShot(clip);
            }
        }
    }

    private void MostrarCartel()
    {
        isCartelVisible = true;
        cartelImage.enabled = true;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeDither(1f, 0f));

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(TemporizadorOcultar());
    }

    private void OcultarCartel()
    {
        isCartelVisible = false;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeDither(0f, 1f, true));

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

    private IEnumerator FadeDither(float startValue, float endValue, bool disableAfter = false)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float cutoffValue = Mathf.Lerp(startValue, endValue, elapsedTime / fadeDuration);
            ditherMaterial.SetFloat("_Cutoff", cutoffValue);
            yield return null;
        }

        ditherMaterial.SetFloat("_Cutoff", endValue);

        if (disableAfter)
            cartelImage.enabled = false;
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
            OcultarCartel();
        }
    }
}