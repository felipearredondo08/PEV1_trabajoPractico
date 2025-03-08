using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Controles controles;
    public Vector2 direccion;

    public int contadorMoneditas = 0;
    public float movementSpeed = 100;
    public float jumpForce = 100f;

    private Rigidbody2D rbody;
    public bool isGrounded = false;
    public bool isMoving = false;
    public float groundRayDist = 1.5f;
    public LayerMask groundLayer;
    public float radius = 0.0f;

    private Animator anim;
    private AudioSource audioSource;
    private SpriteRenderer spr; // SpriteRenderer para cambiar el sprite

    // Coyote Time
    public float coyoteTime = 0.2f;  // Tiempo máximo permitido para saltar después de caer
    private float coyoteTimeCounter; // Contador de coyote time

    // Caída libre
    private bool isFalling = false; // Indica si el personaje está en caída libre
    private float fallTimer = 0f; // Temporizador para la caída libre
    public float fallDelay = 0.5f; // Tiempo antes de cambiar el sprite al caer
    public Sprite fallSprite; // Sprite que se mostrará durante la caída libre

    public static Controller instance;

    bool agarrado = false;
    public Vector3 offset;

    public float multiplicadorChoque = 10;

    Transform tramoAgarrado;

    public float velBalanceo = 10f;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>(); // Inicializar el SpriteRenderer
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        float movY = Input.GetAxisRaw("Vertical");
        float movX = Input.GetAxisRaw("Horizontal");
        flip();
        Walk();

        // Detectar si está en el suelo
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer);

        // Si está en el suelo, reiniciamos el coyote time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            // Si no está en el suelo, reducimos el tiempo restante de coyote time
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Lógica de animaciones
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);

        // Detectar caída libre
        if (rbody.velocity.y < 0 && !isGrounded) // Si la velocidad en Y es negativa y no está en el suelo
        {
            if (!isFalling)
            {
                isFalling = true;
                fallTimer = 0f;
            }
            else
            {
                fallTimer += Time.deltaTime;
                if (fallTimer >= fallDelay)
                {
                    // Cambiar al sprite de caída libre
                    spr.sprite = fallSprite;
                    anim.enabled = false; // Desactivar las animaciones normales
                }
            }
        }
        else if (isFalling) // Si estaba cayendo pero ya no
        {
            // Reanudar las animaciones normales
            isFalling = false;
            anim.enabled = true; // Reanudar las animaciones normales
            spr.sprite = null; // Volver al sprite normal (o dejar que el Animator lo maneje)
        }

        if (agarrado)
        {
            // Posicionar y rotar igual al tramo
            transform.position = tramoAgarrado.transform.position + offset;
            transform.rotation = tramoAgarrado.transform.rotation;

            if (Input.GetButtonDown("Jump"))
            {
                if (movY < 0)
                {
                    seSuelta(); // Solo se suelta sin saltar si el jugador presiona hacia abajo
                }
                else
                {
                    seSueltaYSalta();
                    Jump(); // Salta si no está presionando hacia abajo
                }
            }

            tramoAgarrado.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(movX * velBalanceo, 0);
        }
    }

    void seSuelta()
    {
        agarrado = false;
        rbody.isKinematic = false;
        rbody.velocity = new Vector2(0, 0);
        transform.rotation = Quaternion.identity;
    }

    void seSueltaYSalta()
    {
        agarrado = false;
        rbody.isKinematic = false;
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Cuerda") && Input.GetButtonDown("Fire1"))
        {
            agarrado = true;
            tramoAgarrado = other.transform;
            other.GetComponent<Rigidbody2D>().AddForce(rbody.velocity * multiplicadorChoque, ForceMode2D.Impulse);

            // Suspender la gravedad
            rbody.isKinematic = true;
        }
    }

    public void Walk()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rbody.velocity = new Vector2(horizontalInput * movementSpeed, rbody.velocity.y);
        isMoving = horizontalInput != 0;
    }

    public void Jump()
    {
        // Permitir el salto si está en el suelo o si aún queda tiempo de Coyote Time
        if (coyoteTimeCounter <= 0) return;

        // Aplica la fuerza de salto
        rbody.velocity = Vector2.up * jumpForce;

        audioSource.Play();

        anim.SetTrigger("isJump");

        // Una vez que salte, reseteamos el coyote time para evitar múltiples saltos
        coyoteTimeCounter = 0;

        // Restablecer el sprite y las animaciones si estaba en caída libre
        if (isFalling)
        {
            isFalling = false;
            anim.enabled = true;
            spr.sprite = null; // Volver al sprite normal (o dejar que el Animator lo maneje)
        }
    }

    public void flip()
    {
        float movHor = Input.GetAxis("Horizontal");

        if (movHor > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (movHor < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}