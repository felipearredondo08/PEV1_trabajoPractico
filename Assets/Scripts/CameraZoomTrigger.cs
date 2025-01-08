using UnityEngine;
using Cinemachine;

public class CameraZoomAndOffsetTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomedOutSize ;
    public float transitionSpeed = 2f;
    public float offsetX = 0.0f;
    public float offsetY = 0.0f;
    public Camera parallaxCamera;
    public MonoBehaviour parallaxScript; // Referencia al script de parallax (MonoBehaviour para flexibilidad)

    private float initialZoomSize;
    private float initialScreenX;
    private float initialScreenY;
    private float targetSize;
    private float targetScreenX;
    private float targetScreenY;

    void Start()
    {
        if (virtualCamera != null)
        {
            // Configurar los valores iniciales
            initialZoomSize = virtualCamera.m_Lens.OrthographicSize;
            targetSize = initialZoomSize;

            CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                initialScreenX = framingTransposer.m_ScreenX;
                initialScreenY = framingTransposer.m_ScreenY;
                targetScreenX = initialScreenX;
                targetScreenY = initialScreenY;
            }
        }
    }

    void Update()
    {
        if (virtualCamera != null)
        {
            // Suavizar el zoom
            var currentSize = virtualCamera.m_Lens.OrthographicSize;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * transitionSpeed);

            CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                // Suavizar el desplazamiento en X e Y
                framingTransposer.m_ScreenX = Mathf.Lerp(framingTransposer.m_ScreenX, targetScreenX, Time.deltaTime * transitionSpeed);
                framingTransposer.m_ScreenY = Mathf.Lerp(framingTransposer.m_ScreenY, targetScreenY, Time.deltaTime * transitionSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Establecer valores de zoom y desplazamiento a los definidos en la configuración del trigger
            targetSize = zoomedOutSize;
            targetScreenX = initialScreenX + offsetX; // Basado en el valor inicial
            targetScreenY = initialScreenY + offsetY;

            // Desactivar el parallax
            if (parallaxCamera != null)
            {
                parallaxCamera.enabled = false;
            }
            if (parallaxScript != null)
            {
                parallaxScript.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Restablecer los valores al estado inicial
            targetSize = initialZoomSize;
            targetScreenX = initialScreenX;
            targetScreenY = initialScreenY;

            // Activar el parallax
            if (parallaxCamera != null)
            {
                parallaxCamera.enabled = true;
            }
            if (parallaxScript != null)
            {
                parallaxScript.enabled = true;
            }
        }
    }
}
