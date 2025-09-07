using UnityEngine;  
// Importa la librería principal de Unity, necesaria para trabajar con componentes,
// objetos y sus transformaciones en el motor.

[ExecuteInEditMode]
// Este atributo hace que el script también se ejecute en el modo de edición
// (cuando estás en el editor y no en "Play"). Sirve para visualizar y ajustar
// el efecto de parallax sin necesidad de entrar al modo juego.

public class ParallaxLayer : MonoBehaviour
// Declaramos una clase llamada "ParallaxLayer". Al heredar de MonoBehaviour,
// significa que este script puede ser agregado como componente a un GameObject en Unity.
{
    public float parallaxFactor;
    // Esta variable pública define "qué tanto" se moverá esta capa en comparación
    // con el resto. Un valor bajo (ej: 0.1) hace que se mueva más lento (fondo lejano),
    // y un valor alto (ej: 1 o más) hace que se mueva más rápido (objetos más cercanos).

    public void Move(float delta)
    // Función pública llamada "Move". Recibe un número decimal (delta),
    // que representa cuánto se movió la cámara o el jugador en el eje X.
    {
        Vector3 newPos = transform.localPosition;
        // Guarda en una variable la posición actual del objeto al que está
        // asociado este script (posición local respecto a su padre en la jerarquía).

        newPos.x -= delta * parallaxFactor;
        // Modifica la posición en X. El valor "delta" se multiplica por el factor de parallax:
        // - Si el factor es pequeño, el objeto se moverá poco (sensación de estar lejos).
        // - Si el factor es grande, se moverá más (sensación de cercanía).

        transform.localPosition = newPos;
        // Finalmente, asigna la nueva posición al objeto. Esto aplica el efecto
        // de desplazamiento diferenciado que produce la ilusión de profundidad.
    }
}
