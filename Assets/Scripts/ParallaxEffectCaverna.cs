using UnityEngine;

public class ParallaxEffectCaverna : MonoBehaviour
{
    public Transform player; // Asigna el personaje en el Inspector
    public float parallaxSpeed = 0.5f; // Ajusta la velocidad del efecto

    private float lastPlayerX;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("ParallaxEffect: No se ha asignado el jugador.");
            return;
        }
        lastPlayerX = player.position.x;
    }

    void Update()
    {
        if (player == null) return;

        float deltaX = player.position.x - lastPlayerX; // Diferencia de posición del personaje
        transform.position += new Vector3(-deltaX * parallaxSpeed, 0, 0); // Movimiento en dirección opuesta
        lastPlayerX = player.position.x; // Actualizar la última posición
    }
}
