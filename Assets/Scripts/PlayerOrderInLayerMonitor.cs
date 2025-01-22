using UnityEngine;

public class PlayerOrderInLayerMonitor : MonoBehaviour
{
    private Transform torchTransform; // Transform de la antorcha
    private SpriteRenderer torchRenderer; // Renderer de la antorcha
    private Vector2 offsetRight; // Offset para cuando se mueve a la derecha
    private Vector2 offsetLeft;  // Offset para cuando se mueve a la izquierda
    private int orderRight;      // Orden de capa hacia la derecha
    private int orderLeft;       // Orden de capa hacia la izquierda
    private Rigidbody2D rb2D;    // Referencia al Rigidbody2D del jugador

    public void Initialize(Transform torchTransform, SpriteRenderer torchRenderer, Vector2 offsetRight, Vector2 offsetLeft, int orderRight, int orderLeft)
    {
        this.torchTransform = torchTransform;
        this.torchRenderer = torchRenderer;
        this.offsetRight = offsetRight;
        this.offsetLeft = offsetLeft;
        this.orderRight = orderRight;
        this.orderLeft = orderLeft;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb2D == null || torchTransform == null || torchRenderer == null)
            return;

        // Determinar la dirección del movimiento
        float velocityX = rb2D.velocity.x;

        if (velocityX > 0.1f) // Moviéndose a la derecha
        {
            torchRenderer.sortingOrder = orderRight;
            torchTransform.localPosition = offsetRight; // Aplicar offset hacia la derecha
        }
        else if (velocityX < -0.1f) // Moviéndose a la izquierda
        {
            torchRenderer.sortingOrder = orderLeft;
            torchTransform.localPosition = offsetLeft; // Aplicar offset hacia la izquierda
        }
    }
}
