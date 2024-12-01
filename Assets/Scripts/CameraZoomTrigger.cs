using UnityEngine;
using Cinemachine;

public class CameraZoomAndOffsetTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomedOutSize = 10f; // Tamaño de zoom alejado
    public float transitionSpeed = 2f; // Velocidad de la transición
    public float offsetX = 0.3f; // Desplazamiento horizontal (ScreenX)
    public float offsetY = 0.5f; // Desplazamiento vertical (ScreenY)
    public Camera parallaxCamera; // Para controlar el parallax

    private float initialZoomSize; // Zoom inicial
    private float initialScreenX; // ScreenX inicial
    private float initialScreenY; // ScreenY inicial
    private float targetSize; 
    private float targetScreenX;
    private float targetScreenY;

    void Start()
    {
        if (virtualCamera != null)
        {
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
            targetSize = zoomedOutSize;
            targetScreenX = offsetX;
            targetScreenY = offsetY;

            if (parallaxCamera != null)
            {
                parallaxCamera.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetSize = initialZoomSize;
            targetScreenX = initialScreenX;
            targetScreenY = initialScreenY;

            if (parallaxCamera != null)
            {
                parallaxCamera.enabled = true;
            }
        }
    }
}