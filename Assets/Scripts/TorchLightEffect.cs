using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLightEffect : MonoBehaviour
{
    public Light2D torchLight; // Referencia a la luz 2D
    public float intensityVariation = 0.1f; // Variación en la intensidad
    public float positionNoise = 0.1f; // Movimiento aleatorio en la posición
    public float radiusVariation = 0.05f; // Variación en el radio interno/externo
    public float speed = 5f; // Velocidad del efecto

    private Vector3 initialPosition;
    private float initialIntensity;
    private float initialInnerRadius;
    private float initialOuterRadius;

    void Start()
    {
        if (torchLight != null)
        {
            initialPosition = torchLight.transform.localPosition;
            initialIntensity = torchLight.intensity;
            initialInnerRadius = torchLight.pointLightInnerRadius;
            initialOuterRadius = torchLight.pointLightOuterRadius;
        }
        else
        {
            Debug.LogError("No Light2D assigned to TorchLightEffect script.");
        }
    }

    void Update()
    {
        if (torchLight != null)
        {
            // Variación en la intensidad
            torchLight.intensity = initialIntensity + Mathf.PerlinNoise(Time.time * speed, 0) * intensityVariation;

            // Movimiento aleatorio de la posición
            float offsetX = Mathf.PerlinNoise(Time.time * speed, 1) * positionNoise - (positionNoise / 2);
            float offsetY = Mathf.PerlinNoise(Time.time * speed, 2) * positionNoise - (positionNoise / 2);
            torchLight.transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);

            // Variación en los radios interno y externo
            torchLight.pointLightInnerRadius = initialInnerRadius + Mathf.PerlinNoise(Time.time * speed, 3) * radiusVariation;
            torchLight.pointLightOuterRadius = initialOuterRadius + Mathf.PerlinNoise(Time.time * speed, 4) * radiusVariation;
        }
    }
}