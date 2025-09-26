using UnityEngine;  // Librería principal de Unity

[ExecuteInEditMode]
// Permite que este script funcione también en el editor sin necesidad de estar en Play.
// Así se puede ver el efecto de parallax mientras se edita la escena.
/* 🔑 Cómo se conectan los tres scripts

ParallaxCamera: detecta el movimiento de la cámara y emite el valor del desplazamiento (delta).

ParallaxBackground: escucha a ParallaxCamera y envía ese delta a cada capa.

ParallaxLayer: aplica el movimiento a su GameObject, multiplicado por su parallaxFactor.
*/

public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    // Define un "delegado", que es como un tipo de función que recibe un número (float).
    // En este caso, representará el movimiento de la cámara en el eje X.

    public ParallaxCameraDelegate onCameraTranslate;
    // Evento basado en el delegado. Otras clases (como ParallaxBackground)
    // se suscriben a este evento para "escuchar" cuando la cámara se mueva.

    private float oldPosition;
    // Guarda la posición anterior de la cámara en el eje X,
    // para poder calcular cuánto se movió desde el último frame.

    void Start()
    {
        oldPosition = transform.position.x;
        // Cuando arranca el juego (o en el editor),
        // se guarda la posición inicial de la cámara en X.
    }

    void Update()
    {
        // Se ejecuta en cada frame.
        if (transform.position.x != oldPosition)
        // Verifica si la posición actual en X de la cámara es distinta de la anterior.
        {
            if (onCameraTranslate != null)
            {
                float delta = oldPosition - transform.position.x;
                // Calcula cuánto se movió la cámara desde el último frame.
                // Delta positivo/negativo indica dirección del movimiento.

                onCameraTranslate(delta);
                // Dispara el evento: avisa a todos los suscriptores
                // (por ejemplo, ParallaxBackground) del movimiento.
            }

            oldPosition = transform.position.x;
            // Actualiza la "vieja posición" para el siguiente frame.
        }
    }
}
