/*using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogoNuevo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText; // Referencia al texto de diálogo
    [SerializeField] private Image dialogueBackground; // Referencia al fondo del diálogo
    [SerializeField] private Image[] characterImages; // Referencia a las imágenes de los personajes (3 personajes)
    [SerializeField] private AudioSource audioSource; // Referencia al AudioSource
    [SerializeField] private AudioClip typingSound; // Sonido de tipeo de cada letra
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
    private int currentCharacterIndex = 0; // Índice para saber qué personaje está hablando

    private void Start()
    {
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        dialogueBackground.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        // Aseguramos que solo se muestre una imagen a la vez
        foreach (var image in characterImages)
        {
            image.gameObject.SetActive(false);
        }

        // Mostrar la imagen del primer personaje
        characterImages[currentCharacterIndex].gameObject.SetActive(true);

        while (currentLineIndex < dialogueLines.Length)
        {
            // Cambiar el sprite del personaje
            characterImages[currentCharacterIndex].gameObject.SetActive(true);

            // Mostrar y desvanecer la animación antes de mostrar la línea de diálogo
            yield return StartCoroutine(FadeInOutAnimation(true));

            // Escribir la línea de diálogo
            yield return StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));

            // Esperar antes de pasar a la siguiente línea
            yield return new WaitForSeconds(delayBetweenLines);

            // Desvanecer la animación después de la línea
            yield return StartCoroutine(FadeInOutAnimation(false));

            // Cambiar al siguiente personaje (circularmente)
            currentCharacterIndex = (currentCharacterIndex + 1) % characterImages.Length;

            // Ocultar la imagen anterior del personaje
            foreach (var image in characterImages)
            {
                image.gameObject.SetActive(false);
            }

            // Mostrar la nueva imagen del personaje
            characterImages[currentCharacterIndex].gameObject.SetActive(true);

            currentLineIndex++;
        }

        // Desvanecer el fondo, texto y animación al final del diálogo
        yield return StartCoroutine(FadeOutTextAndBackground());

        // Destruir el objeto después de terminar el diálogo
        if (areaDialogoInicialCamara != null)
        {
            Destroy(areaDialogoInicialCamara);
        }
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
        Color startColor = characterImages[currentCharacterIndex].color;
        float startAlpha = fadeIn ? 0f : startColor.a;  // Si estamos haciendo fade in, comenzamos desde 0

        // Hacer fade in o fade out dependiendo de la acción
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, fadeIn ? 1f : 0f, elapsedTime / fadeDuration);
            characterImages[currentCharacterIndex].color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Asegurar que la animación se quede en el valor final del alfa
        Color finalColor = characterImages[currentCharacterIndex].color;
        characterImages[currentCharacterIndex].color = new Color(finalColor.r, finalColor.g, finalColor.b, fadeIn ? 1f : 0f);
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
    }
}*/





/*using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogoNuevo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image[] characterImages;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private GameObject areaDialogoInicialCamara;
    
    [SerializeField] private GameObject dialogueBackgroundFirst;
    [SerializeField] private GameObject dialogueBackgroundSecond;
    
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogueLinesFirst = {
        "Bienvenido al primer nivel.",
        "Debes recoger todas las monedas \npara completar el desafío.",
        "¡Buena suerte, aventurero!"
    };

    [TextArea(3, 5)]
    [SerializeField] private string[] dialogueLinesSecond = {
        "¡Buen trabajo!",
        "Ahora prepárate para el siguiente desafío.",
        "Recuerda, la práctica hace al maestro."
    };

    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float delayBetweenLines = 2f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float waitBeforeSecondDialogue = 3f;

    private int currentCharacterIndex = 0;

    private void Start()
    {
        StartCoroutine(HandleDialogues());
    }

    private IEnumerator HandleDialogues()
    {
        yield return StartCoroutine(DisplayDialogue(dialogueLinesFirst, dialogueBackgroundFirst));

        if (dialogueBackgroundFirst != null)
        {
            Destroy(dialogueBackgroundFirst);
        }

        yield return new WaitForSeconds(waitBeforeSecondDialogue);
        
        if (dialogueBackgroundSecond != null)
        {
            dialogueBackgroundSecond.SetActive(true);
        }

        yield return StartCoroutine(DisplayDialogue(dialogueLinesSecond, dialogueBackgroundSecond));

        if (areaDialogoInicialCamara != null)
        {
            Destroy(areaDialogoInicialCamara);
        }
    }

    private IEnumerator DisplayDialogue(string[] dialogueLines, GameObject dialogueBackground)
    {
        if (dialogueBackground != null)
        {
            dialogueBackground.SetActive(true);
        }
        
        dialogueText.gameObject.SetActive(true);
        
        foreach (var image in characterImages)
        {
            image.gameObject.SetActive(false);
        }

        characterImages[currentCharacterIndex].gameObject.SetActive(true);

        for (int i = 0; i < dialogueLines.Length; i++)
        {
            characterImages[currentCharacterIndex].gameObject.SetActive(true);
            yield return StartCoroutine(FadeInOutAnimation(true));
            yield return StartCoroutine(TypeLine(dialogueLines[i]));
            yield return new WaitForSeconds(delayBetweenLines);
            yield return StartCoroutine(FadeInOutAnimation(false));

            currentCharacterIndex = (currentCharacterIndex + 1) % characterImages.Length;

            foreach (var image in characterImages)
            {
                image.gameObject.SetActive(false);
            }
            characterImages[currentCharacterIndex].gameObject.SetActive(true);
        }

        yield return StartCoroutine(FadeOutTextAndBackground(dialogueBackground));
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1);

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

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
        Color startColor = characterImages[currentCharacterIndex].color;
        float startAlpha = fadeIn ? 0f : startColor.a;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, fadeIn ? 1f : 0f, elapsedTime / fadeDuration);
            characterImages[currentCharacterIndex].color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Color finalColor = characterImages[currentCharacterIndex].color;
        characterImages[currentCharacterIndex].color = new Color(finalColor.r, finalColor.g, finalColor.b, fadeIn ? 1f : 0f);
    }

    private IEnumerator FadeOutTextAndBackground(GameObject dialogueBackground)
    {
        float elapsedTime = 0f;
        Color originalTextColor = dialogueText.color;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            dialogueText.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, alpha);
            yield return null;
        }

        dialogueText.gameObject.SetActive(false);
        
        if (dialogueBackground != null)
        {
            dialogueBackground.SetActive(false);
        }
    }
}*/

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogoNuevo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    // Imágenes de los personajes para el primer diálogo
    [SerializeField] private Image[] characterImagesFirst;
    
    // Imágenes de los personajes para el segundo diálogo
    [SerializeField] private Image[] characterImagesSecond;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private GameObject areaDialogoInicialCamara;
    
    [SerializeField] private GameObject dialogueBackgroundFirst;
    [SerializeField] private GameObject dialogueBackgroundSecond;
    
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogueLinesFirst = {
        "Bienvenido al primer nivel.",
        "Debes recoger todas las monedas \npara completar el desafío.",
        "¡Buena suerte, aventurero!"
    };

    [TextArea(3, 5)]
    [SerializeField] private string[] dialogueLinesSecond = {
        "¡Buen trabajo!",
        "Ahora prepárate para el siguiente desafío.",
        "Recuerda, la práctica hace al maestro."
    };

    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float delayBetweenLines = 2f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float waitBeforeSecondDialogue = 3f;

    private void Start()
    {
        StartCoroutine(HandleDialogues());
    }

    private IEnumerator HandleDialogues()
    {
        yield return StartCoroutine(DisplayDialogue(dialogueLinesFirst, dialogueBackgroundFirst, characterImagesFirst));

        if (dialogueBackgroundFirst != null)
        {
            Destroy(dialogueBackgroundFirst);
        }

        yield return new WaitForSeconds(waitBeforeSecondDialogue);
        
        if (dialogueBackgroundSecond != null)
        {
            dialogueBackgroundSecond.SetActive(true);
        }

        yield return StartCoroutine(DisplayDialogue(dialogueLinesSecond, dialogueBackgroundSecond, characterImagesSecond));

        if (areaDialogoInicialCamara != null)
        {
            Destroy(areaDialogoInicialCamara);
        }
    }

    private IEnumerator DisplayDialogue(string[] dialogueLines, GameObject dialogueBackground, Image[] characterImages)
    {
        if (dialogueBackground != null)
        {
            dialogueBackground.SetActive(true);
        }
        
        dialogueText.gameObject.SetActive(true);

        // Desactivar todas las imágenes de personajes antes de empezar
        foreach (var image in characterImages)
        {
            image.gameObject.SetActive(false);
        }

        int currentCharacterIndex = 0;
        characterImages[currentCharacterIndex].gameObject.SetActive(true);

        for (int i = 0; i < dialogueLines.Length; i++)
        {
            characterImages[currentCharacterIndex].gameObject.SetActive(true);
            yield return StartCoroutine(FadeInOutAnimation(characterImages[currentCharacterIndex], true));
            yield return StartCoroutine(TypeLine(dialogueLines[i]));
            yield return new WaitForSeconds(delayBetweenLines);
            yield return StartCoroutine(FadeInOutAnimation(characterImages[currentCharacterIndex], false));

            // Cambia de personaje para la siguiente línea
            currentCharacterIndex = (currentCharacterIndex + 1) % characterImages.Length;

            foreach (var image in characterImages)
            {
                image.gameObject.SetActive(false);
            }
            characterImages[currentCharacterIndex].gameObject.SetActive(true);
        }

        yield return StartCoroutine(FadeOutTextAndBackground(dialogueBackground));
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1);

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            if (audioSource != null && typingSound != null)
            {
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(textSpeed);
        }
    }

    private IEnumerator FadeInOutAnimation(Image characterImage, bool fadeIn)
    {
        float elapsedTime = 0f;
        Color startColor = characterImage.color;
        float startAlpha = fadeIn ? 0f : startColor.a;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, fadeIn ? 1f : 0f, elapsedTime / fadeDuration);
            characterImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Color finalColor = characterImage.color;
        characterImage.color = new Color(finalColor.r, finalColor.g, finalColor.b, fadeIn ? 1f : 0f);
    }

    private IEnumerator FadeOutTextAndBackground(GameObject dialogueBackground)
    {
        float elapsedTime = 0f;
        Color originalTextColor = dialogueText.color;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            dialogueText.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, alpha);
            yield return null;
        }

        dialogueText.gameObject.SetActive(false);
        
        if (dialogueBackground != null)
        {
            dialogueBackground.SetActive(false);
        }
    }
}

