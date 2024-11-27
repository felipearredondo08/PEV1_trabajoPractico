using UnityEngine;

public class MovimientoAleatorio : MonoBehaviour
{
    // Variables públicas para controlar la velocidad
    public float velocidadMovimiento = 1.0f;
    public float velocidadRotacion = 10.0f;

    // Variable privada para almacenar la dirección aleatoria
    private Vector3 direccionMovimiento;

    void Start()
    {
        // Inicializa la dirección de movimiento aleatoria
        GenerarNuevaDireccion();
    }

    void Update()
    {
        // Mueve el objeto en la dirección actual
        transform.Translate(direccionMovimiento * velocidadMovimiento * Time.deltaTime);

        // Rota el objeto aleatoriamente
        transform.Rotate(Vector3.forward * velocidadRotacion * Time.deltaTime);

        // Cambia la dirección de movimiento de vez en cuando (ejemplo cada 2 segundos)
        if (Random.Range(0f, 1f) < 0.01f) // 1% de probabilidad de cambiar cada frame
        {
            GenerarNuevaDireccion();
        }
    }

    // Función para generar una nueva dirección aleatoria de movimiento
    void GenerarNuevaDireccion()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        direccionMovimiento = new Vector3(x, y, 0).normalized;
    }
}