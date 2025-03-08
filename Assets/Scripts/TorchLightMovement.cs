using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLightMovement : MonoBehaviour
{
    private Light2D light2D;
    private Vector3 initialLightPosition;
    private float initialIntensity;
    private float randomOffset; // Offset aleatorio para cada antorcha

    [Header("Movimiento de la luz")]
    public float lightMovementAmount = 0.03f; // Amplitud del movimiento en X e Y
    public float lightMovementSpeed = 3f; // Velocidad del movimiento oscilatorio

    [Header("Variación de intensidad")]
    public float intensityVariation = 2f; // Cantidad de variación en la intensidad
    public float intensitySpeed = 1f; // Velocidad del cambio de intensidad
    public float randomIntensityFactor = 2f; // Controla cuán aleatorio es el cambio de intensidad

    private void Start()
    {
        light2D = GetComponent<Light2D>();

        if (light2D == null)
        {
            light2D = GetComponentInChildren<Light2D>(); // Busca en los hijos
        }

        if (light2D == null)
        {
            Debug.LogError("No Light2D found in TorchLightMovement.");
            return;
        }

        initialIntensity = light2D.intensity;
        initialLightPosition = light2D.transform.localPosition;

        randomOffset = Random.Range(0f, 100f); // Offset aleatorio para que cada antorcha tenga un ciclo distinto
    }

    private void Update()
    {
        if (light2D != null && light2D.intensity > 0)
        {
            // Movimiento oscilatorio aleatorio en la luz usando Perlin Noise
            float offsetX = (Mathf.PerlinNoise(Time.time * lightMovementSpeed, randomOffset) - 0.5f) * lightMovementAmount;
            float offsetY = (Mathf.PerlinNoise(randomOffset, Time.time * lightMovementSpeed) - 0.5f) * lightMovementAmount;
            light2D.transform.localPosition = initialLightPosition + new Vector3(offsetX, offsetY, 0);

            // Variación de la intensidad con Perlin Noise para suavizar los cambios
            float noiseValue = Mathf.PerlinNoise(Time.time * intensitySpeed, randomOffset);
            float randomFactor = (noiseValue - 0.5f) * randomIntensityFactor * 2; // Rango ajustado entre -factor y +factor
            light2D.intensity = initialIntensity + randomFactor;
        }
    }
}
