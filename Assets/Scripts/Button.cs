using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Door Settings")]
    public GameObject door; // La puerta que será desactivada

    [Header("Switch Settings")]
    public Sprite switchActivatedSprite; // Sprite que indica el estado activado
    private SpriteRenderer spriteRenderer; // Componente SpriteRenderer del interruptor

    [Header("Audio Settings")]
    public AudioSource audioSource; // Fuente de audio para el sonido de activación

    private bool playerInRange = false;
    private bool isActivated = false; // Para evitar múltiples activaciones

    void Start()
    {
        // Obtén el componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Obtén el componente AudioSource (si no se asigna manualmente)
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            ActivateSwitch();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void ActivateSwitch()
    {
        isActivated = true;

        // Desactiva la puerta
        if (door != null)
        {
            door.SetActive(false);
        }

        // Cambia el sprite del interruptor
        if (spriteRenderer != null && switchActivatedSprite != null)
        {
            spriteRenderer.sprite = switchActivatedSprite;
        }

        // Reproduce el sonido (opcional)
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
