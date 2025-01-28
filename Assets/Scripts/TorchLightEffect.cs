using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchCollisionHandler : MonoBehaviour
{
    private Light2D light2D;

    private void Start()
    {
        light2D = GetComponentInChildren<Light2D>();
        if (light2D == null)
        {
            Debug.LogError("No Light2D found in TorchCollisionHandler.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si colisiona con un objeto con el tag "fuego"
        if (collision.CompareTag("fuego") && light2D != null)
        {
            Debug.Log("Antorcha tocó el fuego");
            light2D.intensity = 6.76f; // Aumentar la intensidad de la luz
            
            Invoke("FadeOutLight", 10f); // Iniciar el apagado después de 10 segundos
        }
    }

    private void FadeOutLight()
    {
        if (light2D != null)
        {
            StartCoroutine(GradualFadeOut());
        }
    }

    private System.Collections.IEnumerator GradualFadeOut()
    {
        float duration = Random.Range(3f, 4f); // Duración del fade out
        float initialIntensity = light2D.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            light2D.intensity = Mathf.Lerp(initialIntensity, 0, elapsedTime / duration);
            yield return null;
        }

        light2D.intensity = 0; // Asegurarse de que termine en 0
    }
}
