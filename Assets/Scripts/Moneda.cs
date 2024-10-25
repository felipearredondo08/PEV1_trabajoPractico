/*using UnityEngine;

public class honguitoMoneda : MonoBehaviour
{
    public AudioClip pickupMushrom; // Asigna "Pickup_mushrom" en el Inspector
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = pickupMushrom;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga la etiqueta "Player"
        {
            audioSource.Play(); // Reproduce el sonido
            Destroy(gameObject, pickupMushrom.length); // Elimina después de que termine el sonido
        }
    }
} */
using UnityEngine;

public class HonguitoMoneda : MonoBehaviour
{
    public AudioClip pickupMushrom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Controller player = other.GetComponent<Controller>();
            if (player != null)
            {
                player.contadorMoneditas++;

                UIManager uiManager = FindObjectOfType<UIManager>();
                if (uiManager != null)
                {
                    uiManager.ActualizarContadorMoneditas(player.contadorMoneditas);
                }
            }

            GameObject sonidoTemporal = new GameObject("SonidoPickup");
            AudioSource audioSource = sonidoTemporal.AddComponent<AudioSource>();
            audioSource.clip = pickupMushrom;
            audioSource.Play();
            Destroy(sonidoTemporal, pickupMushrom.length);

            Destroy(gameObject);
        }
    }
}