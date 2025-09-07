using UnityEngine;  
// Importa la librer�a principal de Unity, necesaria para trabajar con componentes,
// objetos y sus transformaciones en el motor.

[ExecuteInEditMode]
// Este atributo hace que el script tambi�n se ejecute en el modo de edici�n
// (cuando est�s en el editor y no en "Play"). Sirve para visualizar y ajustar
// el efecto de parallax sin necesidad de entrar al modo juego.

public class ParallaxLayer : MonoBehaviour
// Declaramos una clase llamada "ParallaxLayer". Al heredar de MonoBehaviour,
// significa que este script puede ser agregado como componente a un GameObject en Unity.
{
    public float parallaxFactor;
    // Esta variable p�blica define "qu� tanto" se mover� esta capa en comparaci�n
    // con el resto. Un valor bajo (ej: 0.1) hace que se mueva m�s lento (fondo lejano),
    // y un valor alto (ej: 1 o m�s) hace que se mueva m�s r�pido (objetos m�s cercanos).

    public void Move(float delta)
    // Funci�n p�blica llamada "Move". Recibe un n�mero decimal (delta),
    // que representa cu�nto se movi� la c�mara o el jugador en el eje X.
    {
        Vector3 newPos = transform.localPosition;
        // Guarda en una variable la posici�n actual del objeto al que est�
        // asociado este script (posici�n local respecto a su padre en la jerarqu�a).

        newPos.x -= delta * parallaxFactor;
        // Modifica la posici�n en X. El valor "delta" se multiplica por el factor de parallax:
        // - Si el factor es peque�o, el objeto se mover� poco (sensaci�n de estar lejos).
        // - Si el factor es grande, se mover� m�s (sensaci�n de cercan�a).

        transform.localPosition = newPos;
        // Finalmente, asigna la nueva posici�n al objeto. Esto aplica el efecto
        // de desplazamiento diferenciado que produce la ilusi�n de profundidad.
    }
}
