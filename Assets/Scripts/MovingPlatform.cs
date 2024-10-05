using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 3f; // Velocidad de la plataforma
    public float distance = 5f; // Distancia que la plataforma recorrerá
    private Vector3 startPos; // Posición inicial de la plataforma

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position; // Guardar la posición inicial
    }

    // Update is called once per frame
    void Update()
    {
        // Mover la plataforma de un lado a otro
        float movement = Mathf.PingPong(Time.time * speed, distance); // Movimiento oscilante
        transform.position = new Vector3(startPos.x + movement, transform.position.y, transform.position.z);
    }
}