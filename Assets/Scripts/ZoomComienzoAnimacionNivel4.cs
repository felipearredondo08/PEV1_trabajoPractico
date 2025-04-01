using UnityEngine;
using Cinemachine;

public class CameraZoomTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomAmount = 3f; // Más bajo = más zoom

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Activando zoom...");
            virtualCamera.m_Lens.OrthographicSize = zoomAmount;
        }
    }
}
