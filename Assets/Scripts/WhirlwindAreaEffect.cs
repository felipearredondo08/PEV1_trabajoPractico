using UnityEngine;

public class WhirlwindAreaEffect : MonoBehaviour
{
    public Sprite newPlayerSprite; // Sprite que queremos asignar al jugador
    private SpriteRenderer playerSpriteRenderer; // SpriteRenderer del jugador
    private Animator playerAnimator; // Referencia al Animator del jugador
    private bool originalAnimationState; // Estado original de las animaciones

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener el SpriteRenderer y el Animator del jugador
            playerSpriteRenderer = other.GetComponent<SpriteRenderer>();
            playerAnimator = other.GetComponent<Animator>();

            if (playerSpriteRenderer != null && newPlayerSprite != null)
            {
                // Desactivar el Animator para cambiar al sprite estático
                if (playerAnimator != null)
                {
                    originalAnimationState = playerAnimator.enabled; // Guardar estado actual
                    playerAnimator.enabled = false; // Desactivar animaciones
                }

                // Cambiar el sprite del jugador
                playerSpriteRenderer.sprite = newPlayerSprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Restaurar las animaciones originales
            if (playerAnimator != null)
            {
                playerAnimator.enabled = originalAnimationState; // Restaurar estado original
            }
        }
    }
}