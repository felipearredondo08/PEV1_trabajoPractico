using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 2f;
    private int direction = 1; // 1 = Derecha, -1 = Izquierda

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    public float groundCheckOffsetX = 0.2f;
    public float frontRayDistance = 0.3f;

    public GameObject explosionParticlesPrefab; // Prefab de partículas de explosión
    public float bounceForce = 5f; // Fuerza de rebote del jugador

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Verificar si hay suelo adelante
        Vector3 groundRayOrigin = transform.position + Vector3.right * direction * groundCheckOffsetX;
        bool isGroundAhead = Physics2D.Raycast(groundRayOrigin, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(groundRayOrigin, Vector2.down * groundCheckDistance, Color.green);

        // Verificar si hay un enemigo adelante con un RaycastAll para evitar bloqueos
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right * direction, frontRayDistance);
        Debug.DrawRay(transform.position, Vector2.right * direction * frontRayDistance, Color.red);
        bool enemyAhead = false;

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy") && hit.collider.gameObject != gameObject)
            {
                enemyAhead = true;
                break;
            }
        }

        // Si no hay suelo adelante o hay un enemigo, girar
        if (!isGroundAhead || enemyAhead)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    private void Flip()
    {
        direction *= -1; // Invertir dirección
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // Empujar ligeramente en la nueva dirección para evitar bloqueo
        transform.position += new Vector3(direction * 0.03f, 0, 0);
    }

    // Detectar si el jugador cae sobre el enemigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el jugador cae sobre el enemigo
        if (collision.gameObject.CompareTag("Player"))
        {
            // Comprobar si el jugador está cayendo (su velocidad Y es negativa)
            if (collision.relativeVelocity.y < 0)
            {
                // Hacer que el jugador rebote
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
                }

                // Instanciar partículas de explosión
              //  if (explosionParticlesPrefab != null)
               // {
                 //   Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                //}

                 if (explosionParticlesPrefab != null)
               {
                  GameObject explosion = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                     Destroy(explosion, 2f); // 🔹 Se destruirá después de 2 segundos
                  }
                Destroy(gameObject);  // Destruir al enemigo
            }
        }
    }
}