using System.Collections.Generic;
using UnityEngine;

public class OffscreenIndicatorManager : MonoBehaviour
{
    [Header("Scene")]
    public Transform player;
    public UnityEngine.Camera mainCam;
    public Canvas canvas;

    [Header("Setup")]
    public GameObject indicatorPrefab;      // el prefab UI del punto + flecha
    public string tornadoTag = "Tornado";   // etiqueta de los objetivos

    [Header("Rules (por defecto)")]
    public float maxDistance = 60f;
    public float minDistance = 5f;
    public float screenPadding = 24f;

    readonly List<OffscreenIndicator> _active = new();


    [Header("Dynamic range")]
    public bool useDynamicRange = true;
    [Range(1.2f, 2.5f)] public float maxRangeMultiplier = 1.6f; // qué tan “cerca” consideramos útil
    public float innerBuffer = 0.5f; // margen adicional para el borde

    void Start()
    {
        SpawnAll();
    }
    float GetVisibleRadius(UnityEngine.Camera cam, Vector3 atWorldPos)
    {
        if (cam.orthographic)
        {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;
            return Mathf.Sqrt(halfW * halfW + halfH * halfH);
        }
        else
        {
            // aproximamos en el plano del jugador
            float dist = Vector3.Distance(cam.transform.position, atWorldPos);
            float halfH = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * dist;
            float halfW = halfH * cam.aspect;
            return Mathf.Sqrt(halfW * halfW + halfH * halfH);
        }
    }
    void LateUpdate()
    {
        if (!useDynamicRange || mainCam == null || player == null) return;

        float visR = GetVisibleRadius(mainCam, player.position);

        float dynamicMin = visR + innerBuffer;          // apenas más que lo visible
        float dynamicMax = visR * maxRangeMultiplier;   // hasta ~1.6x el cuadro

        for (int i = 0; i < _active.Count; i++)
        {
            var ind = _active[i];
            if (ind == null) continue;
            ind.minDistance = dynamicMin;
            ind.maxDistance = dynamicMax;
        }
    }
    public void SpawnAll()
    {
        // limpiar anteriores
        foreach (var ind in _active) if (ind) Destroy(ind.gameObject);
        _active.Clear();

        // encontrar tornados
        var tornados = GameObject.FindGameObjectsWithTag(tornadoTag);
        foreach (var t in tornados)
        {
            var go = Instantiate(indicatorPrefab, canvas.transform);
            var ind = go.GetComponent<OffscreenIndicator>();
            ind.target = t.transform;
            ind.player = player;
            ind.mainCam = mainCam;
            ind.canvas = canvas;
            ind.maxDistance = maxDistance;
            ind.minDistance = minDistance;
            ind.screenPadding = screenPadding;

            _active.Add(ind);
        }
    }
}
