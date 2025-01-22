using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchPickup : MonoBehaviour
{
    public GameObject torchPrefab; // Prefab de la antorcha animada
    public Vector3 relativePosition; // Posición relativa ajustable desde el inspector
    private Transform playerTransform; // Referencia al transform del personaje

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si el objeto que entra tiene la etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            // Obtener el transform del personaje
            playerTransform = collision.transform;

            // Instanciar el prefab como hijo del personaje
            GameObject torchInstance = Instantiate(torchPrefab, playerTransform);

            // Ajustar la posición relativa
            torchInstance.transform.localPosition = relativePosition;

            // Asignar la etiqueta "antorchaPiso"
            torchInstance.tag = "antorchaPiso";

            // Asegurarse de que tenga un BoxCollider2D configurado como trigger
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
                // Forzar a 0 tras una breve espera para garantizar que se aplique
                StartCoroutine(SetLightIntensity(light2D, 0));
            }

            // Desactivar o destruir el GameObject "antorchaPiso"
            gameObject.SetActive(false); // O Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator SetLightIntensity(Light2D light, float intensity)
    {
        // Esperar un cuadro para asegurar que Unity procese la creación
        yield return null;
        light.intensity = intensity;
    }
}
