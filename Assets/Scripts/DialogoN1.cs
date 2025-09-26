using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogoN1 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText; // Referencia al texto de diálogo
    [SerializeField] private Image dialogueBackground; // Referencia al fondo del diálogo
    [SerializeField] private GameObject animationObject; // Referencia a la imagen de animación
    [SerializeField] private AudioSource audioSource; // Referencia al AudioSource
    [SerializeField] private AudioClip typingSound; // Referencia al sonido a reproducir al escribir cada carácter
    [SerializeField] private GameObject areaDialogoInicialCamara; // Referencia al objeto a destruir
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogueLines = {
        "Bienvenido al primer nivel.",
        "Debes recoger todas las monedas \npara completar el desafío.",
        "¡Buena suerte, aventurero!"
    }; // Líneas de diálogo
    [SerializeField] private float textSpeed = 0.05f; // Velocidad de escritura del texto
    [SerializeField] private float delayBetweenLines = 2f; // Tiempo entre líneas
    [SerializeField] private float fadeDuration = 1f; // Duración del fade

    private int currentLineIndex = 0;

    private void Start()
    {
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        dialogueBackground.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        while (currentLineIndex < dialogueLines.Length)
        {
            // Mostrar y desvanecer la animación antes de mostrar la línea de diálogo
            yield return StartCoroutine(FadeInOutAnimation(true));

            // Escribir la línea de diálogo
            yield return StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));

            // Esperar antes de pasar a la siguiente línea
            yield return new WaitForSeconds(delayBetweenLines);

            // Desvanecer la animación después de la línea
            yield return StartCoroutine(FadeInOutAnimation(false));

            currentLineIndex++;
        }

        // Desvanecer el fondo, texto y animación al final del diálogo
        yield return StartCoroutine(FadeOutTextAndBackground());

        // Destruir el objeto después de terminar el diálogo
       /* if (areaDialogoInicialCamara != null)
        {
            Destroy(areaDialogoInicialCamara);
        }*/
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1);

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            // Reproducir el sonido de escritura
            if (audioSource != null && typingSound != null)
            {
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(textSpeed);
        }
    }

    private IEnumerator FadeInOutAnimation(bool fadeIn)
    {
        float elapsedTime = 0f;
        Color startColor = animationObject.GetComponent<Image>().color;
        float startAlpha = fadeIn ? 0f : startColor.a;  // Si estamos haciendo fade in, comenzamos desde 0

        // Hacer fade in o fade out dependiendo de la acción
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, fadeIn ? 1f : 0f, elapsedTime / fadeDuration);
            animationObject.GetComponent<Image>().color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Asegurar que la animación se quede en el valor final del alfa
        Color finalColor = animationObject.GetComponent<Image>().color;
        animationObject.GetComponent<Image>().color = new Color(finalColor.r, finalColor.g, finalColor.b, fadeIn ? 1f : 0f);
    }

    private IEnumerator FadeOutTextAndBackground()
    {
        float elapsedTime = 0f;
        Color originalTextColor = dialogueText.color;
        Color originalBackgroundColor = dialogueBackground.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            dialogueText.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, alpha);
            dialogueBackground.color = new Color(originalBackgroundColor.r, originalBackgroundColor.g, originalBackgroundColor.b, alpha);
            yield return null;
        }

        // Desactivar todo al final
        dialogueText.gameObject.SetActive(false);
        dialogueBackground.gameObject.SetActive(false);
        animationObject.SetActive(false);
    }
}