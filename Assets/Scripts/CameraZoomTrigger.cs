using UnityEngine;
using Cinemachine;

public class CameraZoomAndOffsetTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomedOutSize = 10f; // Tamaño de zoom alejado
    public float transitionSpeed = 2f; // Velocidad de la transición
    public float offsetX = 0.3f; // Valor entre 0 y 1 para desplazar la cámara en X (0.5f es centrado)
    public Camera parallaxCamera; // Asigna el GameObject con el script Camera.cs aquí

    private float initialZoomSize; // Almacena el tamaño inicial de zoom
    private float initialScreenX; // Almacena la posición inicial de la cámara en X en la pantalla
    private float targetSize;
    private float targetScreenX;

    void Start()
    {
        if (virtualCamera != null)
        {
            // Captura el tamaño inicial de zoom
            initialZoomSize = virtualCamera.m_Lens.OrthographicSize;
            targetSize = initialZoomSize;

            // Captura el valor inicial de ScreenX si tienes un FramingTransposer
            CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                initialScreenX = framingTransposer.m_ScreenX;
                targetScreenX = initialScreenX;
            }
        }
    }

    void Update()
    {
        if (virtualCamera != null)
        {
            // Suaviza el cambio de tamaño de zoom
            var currentSize = virtualCamera.m_Lens.OrthographicSize;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * transitionSpeed);

            // Suaviza el cambio de desplazamiento horizontal en ScreenX
            CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                framingTransposer.m_ScreenX = Mathf.Lerp(framingTransposer.m_ScreenX, targetScreenX, Time.deltaTime * transitionSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Establece el tamaño de zoom alejado y el desplazamiento horizontal
            targetSize = zoomedOutSize;
            targetScreenX = offsetX;

            // Desactiva el script Camera.cs para detener el parallax
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
            // Restaura el tamaño inicial de zoom y la posición horizontal
            targetSize = initialZoomSize;
            targetScreenX = initialScreenX;

            // Reactiva el script Camera.cs para volver a activar el parallax
            if (parallaxCamera != null)
            {
                parallaxCamera.enabled = true;
            }
        }
    }
}