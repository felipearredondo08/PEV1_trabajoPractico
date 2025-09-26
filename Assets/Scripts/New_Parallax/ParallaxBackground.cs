using System.Collections.Generic;  // Permite usar listas (List<T>).
using UnityEngine;  // Librería principal de Unity.



/*🔑 Explicación 

///Este script funciona como “manager” de todas las capas.

En el objeto padre (por ejemplo, un GameObject vacío llamado Background), agregás este script.

Todos los hijos (bosques, montañas, nubes, etc.) deben tener el script ParallaxLayer.cs.

El ParallaxBackground se encarga de:

Detectar las capas hijas.

Escuchar el movimiento de la cámara (ParallaxCamera).

Aplicar el movimiento diferenciado a cada capa según su parallaxFactor.*/


[ExecuteInEditMode]
// Igual que antes, permite que el script funcione también en el modo de edición
// para ver el efecto de parallax en el editor sin necesidad de darle "Play".
public class ParallaxBackground : MonoBehaviour
{
    public ParallaxCamera parallaxCamera;
    // Referencia al objeto que controla la cámara con parallax.
    // Este script "escucha" sus movimientos.

    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
    // Lista que almacenará todas las capas de parallax (objetos con ParallaxLayer.cs).

    public GameObject mainCam;
    // La cámara principal, desde donde se obtiene el script "ParallaxCamera".

    void Start()
    {
        // Si no se asignó la cámara de parallax en el inspector, se busca en la cámara principal.
        if (parallaxCamera == null)
            parallaxCamera = mainCam.GetComponent<ParallaxCamera>();

        // Si la cámara existe, se conecta el método Move a un "evento" de movimiento.
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        // Inicializa la lista de capas, detectando cuáles tienen el script ParallaxLayer.
        SetLayers();
    }

    void SetLayers()
    {
        parallaxLayers.Clear();  // Limpia la lista antes de volver a llenarla.

        // Recorre todos los objetos hijos del GameObject que tenga este script.
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();
            // Busca si el hijo tiene el script ParallaxLayer.

            if (layer != null)
            {
                layer.name = "Layer-" + i; // Renombra el objeto con un índice (útil para ordenarlos).
                parallaxLayers.Add(layer); // Lo agrega a la lista de capas.
            }
        }
    }

    void Move(float delta)
    {
        // Este método se llama cada vez que la cámara se mueve.
        // "delta" indica cuánto se movió la cámara en el eje X.
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
            // Le pide a cada capa que se mueva proporcionalmente a su parallaxFactor.
        }
    }
}
