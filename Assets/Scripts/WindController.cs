using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{

 public float minWindForce = 1f; // Fuerza mínima del viento
    public float maxWindForce = 5f; // Fuerza máxima del viento
    public float windChangeInterval = 3f; // Tiempo entre cambios de dirección de viento
    public Vector2 windDirection; // Dirección actual del viento

    private float windForce; // Fuerza actual del viento
    private float windChangeTimer = 0f; // Temporizador para cambiar el viento
    private Rigidbody2D rbody; // Referencia al Rigidbody2D del personaje


    // Start is called before the first frame update
    void Start()
    {
          rbody = GetComponent<Rigidbody2D>();
    ChangeWind();
    }

    // Update is called once per frame
    void Update()
    {
         // Actualizar el temporizador
    windChangeTimer += Time.deltaTime;

    // Si se supera el intervalo, cambia la dirección y fuerza del viento
    if (windChangeTimer >= windChangeInterval)
    {
        ChangeWind();
        windChangeTimer = 0f; // Reinicia el temporizador
    }
    }

    void ChangeWind()
{
    // Asigna una dirección aleatoria (hacia la derecha o izquierda)
    windDirection = new Vector2(Random.Range(-1f, 1f), 0f).normalized;
    
    // Asigna una fuerza aleatoria dentro del rango
    windForce = Random.Range(minWindForce, maxWindForce);

    // Opcional: Mostrar la dirección y fuerza del viento en la consola para depuración
    Debug.Log("Nueva dirección de viento: " + windDirection + " con fuerza: " + windForce);
}
}
