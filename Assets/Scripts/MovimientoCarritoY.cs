using UnityEngine;

public class MovimientoCarritoY : MonoBehaviour
{
    public float speed = 1f; // Velocidad de la plataforma en el eje Y
    public float distance = 5f; // Distancia que la plataforma recorrerá en el eje Y
    public float horizontalOffset = 0.5f; // Desplazamiento en el eje X

    private Vector3 startPos; // Posición inicial de la plataforma
    private bool movingUp = true; // Controla si la plataforma se mueve hacia arriba
    private bool isAtEdge = false; // Controla si la plataforma está en un extremo

    private void Start()
    {
        startPos = transform.position; // Guardar la posición inicial
    }

    private void Update()
    {
        float verticalMove = speed * Time.deltaTime * (movingUp ? 1 : -1);
        transform.position += new Vector3(0, verticalMove, 0);

        if (movingUp && transform.position.y >= startPos.y + distance)
        {
            movingUp = false;
            StartCoroutine(MoveHorizontal(horizontalOffset));
        }
        else if (!movingUp && transform.position.y <= startPos.y)
        {
            movingUp = true;
            StartCoroutine(MoveHorizontal(-horizontalOffset));
        }
    }

    private System.Collections.IEnumerator MoveHorizontal(float offset)
    {
        if (isAtEdge) yield break;

        isAtEdge = true;
        Vector3 targetPos = new Vector3(startPos.x + offset, transform.position.y, transform.position.z);
        float elapsedTime = 0f;
        float duration = 0.5f;

        Vector3 initialPos = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isAtEdge = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
