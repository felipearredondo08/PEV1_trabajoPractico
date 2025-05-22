/*using UnityEngine;
using Cinemachine;
using System.Collections;

public class ZoomComienzoAnimacionNivel4 : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform player;

    [Header("Screen X Settings")]
    public float targetScreenX = 0.22f;
    public float resetScreenX = 0.5f;
    public float finalScreenX = 0.4f; // Posición final después de 35s

    [Header("Height Adjustment (Y)")]
    public float targetOffsetY = 2f;
    public float resetOffsetY = 0f;
    public float finalOffsetY = 1f; // Posición final en Y después de 35s

    [Header("Zoom Settings")]
    public float zoomOutAmount = 0.2f;
    private float defaultZoom;

    [Header("Speed Settings")]
    public float detectionThreshold = 0.1f;
    public float transitionSpeed = 2f;

    private CinemachineFramingTransposer framingTransposer;
    private bool isAdjusted = false;

    private void Start()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        defaultZoom = virtualCamera.m_Lens.OrthographicSize;

        // Aseguramos que la cámara comience en la posición inicial deseada
        framingTransposer.m_ScreenX = resetScreenX;
        framingTransposer.m_TrackedObjectOffset.y = resetOffsetY;

        StartCoroutine(AdjustCameraAfterTime());
    }

    private void Update()
    {
        float velocityX = player.GetComponent<Rigidbody2D>().velocity.x;

        if (!isAdjusted && Mathf.Abs(velocityX) < detectionThreshold)
        {
            isAdjusted = true;
            StopAllCoroutines();
            StartCoroutine(AdjustCamera(targetScreenX, targetOffsetY));
        }
        else if (isAdjusted && Mathf.Abs(velocityX) > detectionThreshold)
        {
            isAdjusted = false;
            StopAllCoroutines();
            StartCoroutine(AdjustCamera(resetScreenX, resetOffsetY));
        }
    }

    private IEnumerator AdjustCamera(float newScreenX, float newOffsetY)
    {
        while (Mathf.Abs(framingTransposer.m_ScreenX - newScreenX) > 0.01f ||
               Mathf.Abs(framingTransposer.m_TrackedObjectOffset.y - newOffsetY) > 0.01f)
        {
            framingTransposer.m_ScreenX = Mathf.Lerp(framingTransposer.m_ScreenX, newScreenX, Time.deltaTime * transitionSpeed);
            framingTransposer.m_TrackedObjectOffset.y = Mathf.Lerp(framingTransposer.m_TrackedObjectOffset.y, newOffsetY, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        framingTransposer.m_ScreenX = newScreenX;
        framingTransposer.m_TrackedObjectOffset.y = newOffsetY;
    }

    private IEnumerator AdjustCameraAfterTime()
    {
        yield return new WaitForSeconds(35f);
        Debug.Log("🔹 Pasaron 35s: Ajustando cámara final...");

        StartCoroutine(SmoothAdjustScreenX(finalScreenX));
        StartCoroutine(SmoothAdjustOffsetY(finalOffsetY));
        StartCoroutine(SmoothAdjustZoom(defaultZoom + zoomOutAmount));
    }

    private IEnumerator SmoothAdjustScreenX(float newScreenX)
    {
        while (Mathf.Abs(framingTransposer.m_ScreenX - newScreenX) > 0.01f)
        {
            framingTransposer.m_ScreenX = Mathf.Lerp(framingTransposer.m_ScreenX, newScreenX, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        framingTransposer.m_ScreenX = newScreenX;
    }

    private IEnumerator SmoothAdjustOffsetY(float newOffsetY)
    {
        while (Mathf.Abs(framingTransposer.m_TrackedObjectOffset.y - newOffsetY) > 0.01f)
        {
            framingTransposer.m_TrackedObjectOffset.y = Mathf.Lerp(framingTransposer.m_TrackedObjectOffset.y, newOffsetY, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        framingTransposer.m_TrackedObjectOffset.y = newOffsetY;
    }

    private IEnumerator SmoothAdjustZoom(float targetZoom)
    {
        while (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - targetZoom) > 0.01f)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        virtualCamera.m_Lens.OrthographicSize = targetZoom;
    }
}*/












 ///////////////////////////Este funciona perfecto pero no tiene el fade in //////////////
using UnityEngine;
using Cinemachine;
using System.Collections;

public class ZoomComienzoAnimacionNivel4 : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform player;

    [Header("Screen X Settings")]
    public float initialScreenX = 0.22f;  // Donde inicia (esquina)
    public float finalScreenX = 0.5f;     // Donde debe quedar después de 35s (centrado)

    [Header("Height Adjustment (Y)")]
    public float initialOffsetY = 2f;  // Altura inicial
    public float finalOffsetY = 0f;    // Altura después de 35s

    [Header("Zoom Settings")]
    public float initialZoom = 5f;  // Zoom inicial
    public float finalZoom = 5.2f;  // Zoom después de 35s (más alejado)

    [Header("Speed Settings")]
    public float transitionSpeed = 2f;

    private CinemachineFramingTransposer framingTransposer;

    private void Start()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        // Configurar la cámara en la posición inicial correcta
        framingTransposer.m_ScreenX = initialScreenX;
        framingTransposer.m_TrackedObjectOffset.y = initialOffsetY;
        virtualCamera.m_Lens.OrthographicSize = initialZoom;

        // Iniciar la transición después de 35 segundos
        StartCoroutine(AdjustCameraAfterTime());
    }

    private IEnumerator AdjustCameraAfterTime()
    {
        yield return new WaitForSeconds(35f);
        Debug.Log("🔹 Pasaron 35s: Ajustando cámara a posición final...");

        // Iniciar la transición a la nueva posición
        StartCoroutine(SmoothAdjustScreenX(finalScreenX));
        StartCoroutine(SmoothAdjustOffsetY(finalOffsetY));
        StartCoroutine(SmoothAdjustZoom(finalZoom));
    }

    private IEnumerator SmoothAdjustScreenX(float newScreenX)
    {
        while (Mathf.Abs(framingTransposer.m_ScreenX - newScreenX) > 0.01f)
        {
            framingTransposer.m_ScreenX = Mathf.Lerp(framingTransposer.m_ScreenX, newScreenX, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        framingTransposer.m_ScreenX = newScreenX;
    }

    private IEnumerator SmoothAdjustOffsetY(float newOffsetY)
    {
        while (Mathf.Abs(framingTransposer.m_TrackedObjectOffset.y - newOffsetY) > 0.01f)
        {
            framingTransposer.m_TrackedObjectOffset.y = Mathf.Lerp(framingTransposer.m_TrackedObjectOffset.y, newOffsetY, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        framingTransposer.m_TrackedObjectOffset.y = newOffsetY;
    }

    private IEnumerator SmoothAdjustZoom(float targetZoom)
    {
        while (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - targetZoom) > 0.01f)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        virtualCamera.m_Lens.OrthographicSize = targetZoom;
    }
}
//////////////////////////////////////////////////////////////////

