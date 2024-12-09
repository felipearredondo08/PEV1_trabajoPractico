using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plataformaMaritima : MonoBehaviour
{
    public float speed = 1f; // Velocidad de la plataforma
    public float distance = 1f; // Distancia que la plataforma recorrerá
    private Vector3 startPos; // Posición inicial de la plataforma
    private List<Transform> objetosSobrePlataforma = new List<Transform>(); // Lista de objetos sobre la plataforma

    void Start()
    {
        startPos = transform.position; // Guardar la posición inicial
    }

    void Update()
    {
        // Mover la plataforma de un lado a otro
        float movement = Mathf.PingPong(Time.time * speed, distance); // Movimiento oscilante
        Vector3 nuevaPosicion = new Vector3(startPos.x - movement, transform.position.y, transform.position.z);
        Vector3 deltaMovimiento = nuevaPosicion - transform.position; // Cambio de posición
        

        transform.position = nuevaPosicion;

        // Mover los objetos sobre la plataforma
        foreach (Transform objeto in objetosSobrePlataforma)
        {
            if (objeto != null)
            {
                objeto.position += deltaMovimiento; // Aplicar el mismo movimiento
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Añadir el objeto a la lista si no está ya
        if (!objetosSobrePlataforma.Contains(collision.transform))
        {
            objetosSobrePlataforma.Add(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Eliminar el objeto de la lista al salir de contacto
        if (objetosSobrePlataforma.Contains(collision.transform))
        {
            objetosSobrePlataforma.Remove(collision.transform);
        }
    }
}
