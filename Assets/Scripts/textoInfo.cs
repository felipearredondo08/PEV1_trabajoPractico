using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textoInfo : MonoBehaviour
{
   public float amplitude = 0.5f; // Cuánto se moverá el texto hacia arriba y hacia abajo
    public float frequency = 1f;   // Qué tan rápido se moverá el texto

    private Vector3 startPosition;

    void Start()
    {
        // Guardar la posición inicial del texto
        startPosition = transform.position;
    }

    void Update()
    {
        // Aplicar un movimiento suave hacia arriba y hacia abajo
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}
