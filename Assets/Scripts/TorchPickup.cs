using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchPickup : MonoBehaviour
{
    public GameObject torchPrefab; // Prefab de la antorcha animada
    public AudioClip pickupSound; // Clip de sonido a reproducir
    public float soundVolume = 1.0f; // Volumen del sonido

    private Transform playerTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reproducir el sonido en la posición del objeto antes de destruirlo
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
            }

            // Obtener el transform del personaje
            playerTransform = collision.transform;

            // Instanciar el prefab como hijo del personaje
            GameObject torchInstance = Instantiate(torchPrefab, playerTransform);
            torchInstance.tag = "antorchaPiso";

            // Asegurar que tenga un BoxCollider2D como trigger
            BoxCollider2D boxCollider = torchInstance.GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                boxCollider = torchInstance.AddComponent<BoxCollider2D>();
            }
            boxCollider.isTrigger = true;

            // Configurar la intensidad inicial de la luz a 0
            Light2D light2D = torchInstance.GetComponentInChildren<Light2D>();
            if (light2D != null)
            {
                StartCoroutine(SetLightIntensity(light2D, 0));
            }

            // Desactivar o destruir el GameObject
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator SetLightIntensity(Light2D light, float intensity)
    {
        yield return null;
        light.intensity = intensity;
    }
}