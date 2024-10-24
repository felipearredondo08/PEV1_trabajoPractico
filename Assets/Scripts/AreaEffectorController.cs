/*using UnityEngine;

public class AreaEffectorController : MonoBehaviour
{
    private AreaEffector2D areaEffector;

    void Start()
    {
        // Obtén la referencia al componente Area Effector 2D en el GameObject llamado "area"
        GameObject area = GameObject.Find("area");
        if (area != null)
        {
            areaEffector = area.GetComponent<AreaEffector2D>();

            if (areaEffector != null)
            {
                // Modificar las propiedades del Area Effector 2D
                areaEffector.forceMagnitude = 10f; // Cambiar la magnitud de la fuerza
                areaEffector.forceAngle = 45f; // Cambiar el ángulo de la fuerza
                areaEffector.drag = 1f; // Cambiar el valor de la resistencia
            }
            else
            {
                Debug.LogError("No se encontró el componente Area Effector 2D en el GameObject 'area'.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el GameObject llamado 'area'.");
        }
    }
}*/