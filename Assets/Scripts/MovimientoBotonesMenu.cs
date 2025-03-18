using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoBotonesMenu : MonoBehaviour
{
    public float speed = 1f; // Velocidad del movimiento
    public float distance = 1f; // Distancia del movimiento
    private Vector3 startPos; // Posición inicial
    private float offset; // Desfase único para cada botón

    void Start()
    {
        startPos = transform.position;
        offset = Random.Range(0f, Mathf.PI * 2); // Desfase aleatorio en radianes
    }

    void Update()
    {
        // Se agrega el desfase a la función de PingPong para que no sean idénticos
        float movement = Mathf.PingPong((Time.time + offset) * speed, distance);
        transform.position = new Vector3(startPos.x - movement, transform.position.y, transform.position.z);
    }
}
