using UnityEngine;

public class MovimientoCarrito : MonoBehaviour
{
    public float speed = 1f; // Velocidad de la plataforma
    public float distance = 5f; // Distancia que la plataforma recorrerá en el eje X
    public float verticalOffset = 0.5f; // Distancia que baja o sube en el eje Y

    private Vector3 startPos; // Posición inicial de la plataforma
    private bool movingRight = true; // Controla si la plataforma se mueve hacia la derecha
    private bool isAtEdge = false; // Controla si la plataforma está en un extremo

    private void Start()
    {
        startPos = transform.position; // Guardar la posición inicial
    }

    private void Update()
    {
        // Movimiento horizontal
        float horizontalMove = speed * Time.deltaTime * (movingRight ? 1 : -1);
        transform.position += new Vector3(horizontalMove, 0, 0);

        // Detectar extremos
        if (movingRight && transform.position.x >= startPos.x + distance)
        {
            // Al llegar al extremo derecho
            movingRight = false;
            StartCoroutine(MoveVertical(-verticalOffset)); // Bajar
        }
        else if (!movingRight && transform.position.x <= startPos.x)
        {
            // Al llegar al extremo izquierdo
            movingRight = true;
            StartCoroutine(MoveVertical(verticalOffset)); // Subir
        }
    }

    private System.Collections.IEnumerator MoveVertical(float offset)
    {
        if (isAtEdge) yield break; // Evitar múltiples corrutinas simultáneas

        isAtEdge = true; // Bloquear nuevas corrutinas mientras se ejecuta esta
        Vector3 targetPos = new Vector3(transform.position.x, startPos.y + offset, transform.position.z);
        float elapsedTime = 0f;
        float duration = 0.5f; // Tiempo que tarda en moverse verticalmente

        Vector3 initialPos = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // Asegurarse de que llegue a la posición final
        isAtEdge = false; // Desbloquear
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Hacer al jugador hijo de la plataforma
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Desvincular al jugador de la plataforma
            collision.transform.SetParent(null);
        }
    }
}
