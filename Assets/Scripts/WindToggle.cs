/*using UnityEngine;
using System.Collections;

public class ChifleteToggle : MonoBehaviour
{
    public GameObject chiflete; // Asigna el GameObject "Chiflete" desde el inspector
    public float minActiveTime = 5f; // Tiempo mínimo que debe estar activo
    public float minInactiveTime = 1f; // Tiempo mínimo que debe estar inactivo
    public float maxInactiveTime = 5f; // Tiempo máximo que debe estar inactivo

    private ParticleSystem particleSystemChiflete; // Referencia al ParticleSystem de Chiflete

    private void Start()
    {
        // Obtiene el componente ParticleSystem del GameObject Chiflete
        particleSystemChiflete = chiflete.GetComponent<ParticleSystem>();

        // Inicia el ciclo de encendido y apagado
        StartCoroutine(ToggleChiflete());
    }

    private IEnumerator ToggleChiflete()
    {
        while (true)
        {
            // Activa "Chiflete" y todos sus hijos
            SetChifleteActive(true);

            // Mantén "Chiflete" activo por al menos el tiempo mínimo
            yield return new WaitForSeconds(minActiveTime);

            // Apaga "Chiflete" y todos sus hijos
            SetChifleteActive(false);

            // Espera un tiempo aleatorio antes de volver a encenderlo
            float inactiveTime = Random.Range(minInactiveTime, maxInactiveTime);
            yield return new WaitForSeconds(inactiveTime);
        }
    }

    private void SetChifleteActive(bool state)
    {
        // Activa o desactiva todos los hijos de "Chiflete"
        foreach (Transform child in chiflete.transform)
        {
            child.gameObject.SetActive(state);
        }

        // Activa o desactiva el ParticleSystem del propio Chiflete
        if (particleSystemChiflete != null)
        {
            if (state)
            {
                particleSystemChiflete.Play(); // Activa el ParticleSystem
            }
            else
            {
                particleSystemChiflete.Stop(); // Detiene el ParticleSystem
            }
        }
    }
}*/
using UnityEngine;
using System.Collections;

public class ChifleteToggle : MonoBehaviour
{
    public GameObject chiflete; // GameObject que contiene el viento y los colliders
    public float minActiveTime = 5f;
    public float minInactiveTime = 1f;
    public float maxInactiveTime = 5f;

    public GameObject player; // Referencia al personaje
    public Sprite newSprite; // Nuevo sprite cuando el chiflete está activo
    private Sprite originalSprite;
    private bool isChifleteActive = false; // Saber si chiflete está activo

    private SpriteRenderer playerSpriteRenderer;
    private Animator playerAnimator;
    private ParticleSystem particleSystemChiflete;
    private Collider2D[] areaColliders; // Lista de colliders dentro de "chiflete"

    private void Start()
    {
        // Obtiene el ParticleSystem del chiflete
        if (chiflete != null)
        {
            particleSystemChiflete = chiflete.GetComponent<ParticleSystem>();

            // Busca todos los Collider2D dentro de "chiflete"
            areaColliders = chiflete.GetComponentsInChildren<Collider2D>();
            if (areaColliders.Length == 0)
            {
                Debug.LogError("❌ No se encontraron Colliders en 'chiflete'. Verifica que los colliders estén en los hijos de " + chiflete.name);
            }
        }
        else
        {
            Debug.LogError("❌ No se asignó el GameObject 'chiflete'.");
            return;
        }

        // Obtiene los componentes del jugador
        if (player != null)
        {
            playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            playerAnimator = player.GetComponent<Animator>();

            if (playerSpriteRenderer != null)
            {
                originalSprite = playerSpriteRenderer.sprite;
            }
            else
            {
                Debug.LogError("❌ No se encontró SpriteRenderer en el jugador.");
            }
        }
        else
        {
            Debug.LogError("❌ No se asignó el GameObject del jugador.");
            return;
        }

        // Inicia el ciclo de activación y desactivación del chiflete
        StartCoroutine(ToggleChiflete());
    }

    private IEnumerator ToggleChiflete()
    {
        while (true)
        {
            // Activa "Chiflete"
            SetChifleteActive(true);
            yield return new WaitForSeconds(minActiveTime);

            // Desactiva "Chiflete"
            SetChifleteActive(false);
            float inactiveTime = Random.Range(minInactiveTime, maxInactiveTime);
            yield return new WaitForSeconds(inactiveTime);
        }
    }

    private void SetChifleteActive(bool state)
    {
        isChifleteActive = state;

        // Activa o desactiva los hijos del chiflete
        foreach (Transform child in chiflete.transform)
        {
            child.gameObject.SetActive(state);
        }

        // Controla el sistema de partículas
        if (particleSystemChiflete != null)
        {
            if (state)
                particleSystemChiflete.Play();
            else
                particleSystemChiflete.Stop();
        }
    }

    private void Update()
    {
        // Verifica si el jugador está dentro de alguno de los colliders del chiflete
        if (player != null && IsPlayerInsideArea())
        {
            if (isChifleteActive)
            {
                ApplyWindEffect();
            }
            else
            {
                RestorePlayerSprite();
            }
        }
        else
        {
            RestorePlayerSprite();
        }
    }

    private bool IsPlayerInsideArea()
    {
        if (areaColliders == null || areaColliders.Length == 0)
            return false;

        foreach (Collider2D collider in areaColliders)
        {
            if (collider.bounds.Contains(player.transform.position))
            {
                return true; // Si el jugador está dentro de algún collider, devuelve true
            }
        }
        return false; // Si no está en ningún collider, devuelve false
    }

    private void ApplyWindEffect()
    {
        if (playerSpriteRenderer != null && playerSpriteRenderer.sprite != newSprite)
        {
            playerAnimator.enabled = false; // Detener animaciones
            playerSpriteRenderer.sprite = newSprite; // Cambiar sprite
        }
    }

    private void RestorePlayerSprite()
    {
        if (playerSpriteRenderer != null && playerSpriteRenderer.sprite != originalSprite)
        {
            playerSpriteRenderer.sprite = originalSprite; // Restaurar sprite original
            playerAnimator.enabled = true; // Reactivar animaciones
        }
    }
}
