/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public int contadorMoneditas = 0;
    public float movementSpeed = 100; //definimos una variable para la velocidad del movimiento
    public float jumpForce = 100f; //definimos una variable para luego multiplicar la fuerza de salto.

    private Rigidbody2D rbody; // definimos la variable donde se guardara el componente Rigidbody2d de unity
    public bool isGrounded = false; //creamos un booleano donde decimos que esta tocando el suelo es falso
    public bool isMoving = false;
    public float groundRayDist = 1.5f; //creamos una variable para multiplicar el largo del rayo que nos indicara si es suelo o no.
    public LayerMask groundLayer; //definimos una variable donde guardar el nombre de la etiqueta de "suelo".
    public float radius = 0.0f; //creamos una variable para achicar o contraer el radio del circulo que hara contacto con el suelo.

    private Animator anim;
    private SpriteRenderer spr;
    private AudioSource audioSource;

    // WIND CONTROLLER
    public Vector2 windDirection; // Dirección actual del viento
    public float windForce = 0f; // Fuerza actual del viento
    public float minWindForce = 5000f; // Fuerza mínima del viento
    public float maxWindForce = 10000f; // Fuerza máxima del viento
    public float windChangeInterval = 3f; // Intervalo de tiempo para cambiar el viento
    private float windChangeTimer = 0f; // Temporizador para cambiar el viento      

    public static Controller instance;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //traemos el componente Rigidbody2d y lo guardamos en la variable que habiamos creado.
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        // Asigna la referencia estática al objeto actual
        instance = this;
    }

    void Update()
    {
        flip();
        Walk();

        isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Lógica de animaciones
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);

        // Actualizar el temporizador para el cambio de viento
        windChangeTimer += Time.deltaTime;
        if (windChangeTimer >= windChangeInterval)
        {
            ChangeWind();
            windChangeTimer = 0f;
        }
    }

    public void Walk()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rbody.velocity = new Vector2(horizontalInput * movementSpeed, rbody.velocity.y);

        // Si el input horizontal no es 0, el personaje se está moviendo
        isMoving = horizontalInput != 0;
    }

    public void Jump()
    {
        if (!isGrounded) return; // Solo salta si está en el suelo

        // Aplica la fuerza de salto
        rbody.velocity = Vector2.up * jumpForce;

        // Aplica la fuerza del viento
        rbody.AddForce(windDirection * windForce, ForceMode2D.Impulse);
        audioSource.Play();

        anim.SetTrigger("isJump");
    }

    public void flip()
    {
        float movHor = Input.GetAxis("Horizontal");

        if (movHor > 0)
        {
            // Si se mueve a la derecha, la escala x debe ser positiva
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (movHor < 0)
        {
            // Si se mueve a la izquierda, la escala x debe ser negativa (reflejar)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void ChangeWind()
    {
        // Cambia la dirección aleatoria hacia izquierda o derecha
        windDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // Cambia la fuerza del viento en un rango
        windForce = Random.Range(minWindForce, maxWindForce);

        Debug.Log("Nueva dirección de viento: " + windDirection + " con fuerza: " + windForce);
    }

    void OnDrawGizmos()
    {
        // Solo dibuja el gizmo si hay viento
        if (windForce > 0)
        {
            Gizmos.color = Color.blue;
            Vector3 startPosition = transform.position;
            Vector3 windVector = new Vector3(windDirection.x, windDirection.y, 0) * windForce;

            // Dibuja la dirección y magnitud del viento
            Gizmos.DrawLine(startPosition, startPosition + windVector);
            Gizmos.DrawSphere(startPosition + windVector, 0.2f); // Dibuja una esfera pequeña en la punta de la dirección del viento
        }
    }
}*/

/////////////////codigo nuevo con coyote time ////////////////
///
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

    // Coyote Time
    public float coyoteTime = 0.2f;  // Tiempo máximo permitido para saltar después de caer
    private float coyoteTimeCounter; // Contador de coyote time

    // WIND CONTROLLER
    public Vector2 windDirection; 
    public float windForce = 0f; 
    public float minWindForce = 5000f; 
    public float maxWindForce = 10000f; 
    public float windChangeInterval = 3f; 
    private float windChangeTimer = 0f;     

    public static Controller instance;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        instance = this;

      
    }

////////////////////////////////////////////////////
    
    void Update()
    {
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

        // Actualizar el temporizador para el cambio de viento
        windChangeTimer += Time.deltaTime;
        if (windChangeTimer >= windChangeInterval)
        {
            ChangeWind();
            windChangeTimer = 0f;
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

        // Aplica la fuerza del viento
        rbody.AddForce(windDirection * windForce, ForceMode2D.Impulse);
        audioSource.Play();

        anim.SetTrigger("isJump");

        // Una vez que salte, reseteamos el coyote time para evitar múltiples saltos
        coyoteTimeCounter = 0;
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

    void ChangeWind()
    {
        windDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        windForce = Random.Range(minWindForce, maxWindForce);
        Debug.Log("Nueva dirección de viento: " + windDirection + " con fuerza: " + windForce);
    }

    void OnDrawGizmos()
    {
        if (windForce > 0)
        {
            Gizmos.color = Color.blue;
            Vector3 startPosition = transform.position;
            Vector3 windVector = new Vector3(windDirection.x, windDirection.y, 0) * windForce;

            Gizmos.DrawLine(startPosition, startPosition + windVector);
            Gizmos.DrawSphere(startPosition + windVector, 0.2f);
        }
    }
}