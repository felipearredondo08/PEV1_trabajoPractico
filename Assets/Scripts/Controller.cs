using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public float fuezaGolpe;

    private bool puedeMoverse = true;

    // Nuevas variables para los sprites de balanceo
    public Sprite spriteBalanceoDerecha; // Sprite cuando se balancea a la derecha
    public Sprite spriteBalanceoIzquierda; // Sprite cuando se balancea a la izquierda
    public float maxVelYDespuesDelGolpe = 5f;

    public Sprite spriteGolpeado;

    public float duracionGolpe = 0.5f; // Tiempo en segundos que se verá el sprite golpeado

    private bool estaGolpeado = false;



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
            // Desactivar las animaciones normales
            anim.enabled = false;

            // Posicionar y rotar igual al tramo
            transform.position = tramoAgarrado.transform.position + offset;
            transform.rotation = tramoAgarrado.transform.rotation;

            // Cambiar el sprite según la dirección del balanceo
            if (movX > 0)
            {
                spr.sprite = spriteBalanceoDerecha; // Sprite para balanceo a la derecha
            }
            else if (movX < 0)
            {
                spr.sprite = spriteBalanceoIzquierda; // Sprite para balanceo a la izquierda
            }

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

        if (puedeMoverse == false)
        {
            return;
        }
    }

    void seSuelta()
    {
        agarrado = false;
        rbody.isKinematic = false;
        rbody.velocity = new Vector2(0, 0);
        transform.rotation = Quaternion.identity;
        spr.sprite = null; // Restablecer el sprite al soltarse
        anim.enabled = true; // Reactivar las animaciones normales
    }

    void seSueltaYSalta()
    {
        agarrado = false;
        rbody.isKinematic = false;
        transform.rotation = Quaternion.identity;
        spr.sprite = null; // Restablecer el sprite al soltarse
        anim.enabled = true; // Reactivar las animaciones normales
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

            // Desactivar las animaciones normales
            anim.enabled = false;
        }
    }

    /*public void Walk()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rbody.velocity = new Vector2(horizontalInput * movementSpeed, rbody.velocity.y);
        isMoving = horizontalInput != 0;
    }*/

    public void Walk()
    {
        if (!puedeMoverse) return;

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

    public bool EstaEnSuelo()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector2.down, groundRayDist, groundLayer);

        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector2.down * groundRayDist, Color.green);
            return true;
        }

        Debug.DrawRay(transform.position, Vector2.down * groundRayDist, Color.red);
        return false;
    }

    /* public void AplicarGolpe()
     {

         rbody.velocity = new Vector2(rbody.velocity.x, 0);
         Vector2 direccionGolpe;

         puedeMoverse = false;

         // Cambiar al sprite de golpeado y desactivar animaciones
         spr.sprite = spriteGolpeado;
         anim.enabled = false;

         if (rbody.velocity.x > 0)
         {
             direccionGolpe = new Vector2(-1, 1);

         }
         else
         {
             direccionGolpe = new Vector2(1, 1);
         }

         rbody.AddForce(direccionGolpe * fuezaGolpe);



        if (rbody.velocity.y > maxVelYDespuesDelGolpe)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, maxVelYDespuesDelGolpe); //supuestamente esta linea evitaria que se vaya volando alto pipina.
             
         }

           StopAllCoroutines(); // Detiene otras posibles corutinas para evitar solapamientos
             StartCoroutine(CambiarSpritePorGolpe()); ////////////////////////////////////////////////////////////////////////////////

         StartCoroutine(EsperarYActivarMovimiento());
     }

     IEnumerator CambiarSpritePorGolpe()
{
    anim.enabled = false; // Desactiva el animator para mostrar solo el sprite golpeado
    spr.sprite = spriteGolpeado;

    yield return new WaitForSeconds(duracionGolpe);

    anim.enabled = true; // Reactiva animaciones normales
    spr.sprite = null;   // Deja que el animator recupere el sprite según el estado
}

   
    IEnumerator EsperarYActivarMovimiento()
  {
      yield return new WaitForSeconds(0.1f);
      while (!EstaEnSuelo())
      {
          yield return null;
      }

      puedeMoverse = true;

      // Restaurar animaciones normales
      spr.sprite = null; // Deja que el Animator controle el sprite
      anim.enabled = true;
  }*/

    public void AplicarGolpe()
    {
        rbody.velocity = new Vector2(rbody.velocity.x, 0);
        Vector2 direccionGolpe;

        puedeMoverse = false;

        // Cancelar animaciones antes del sprite
        anim.enabled = false;
        spr.sprite = spriteGolpeado;

        // Determinar dirección del golpe
        if (rbody.velocity.x > 0)
        {
            direccionGolpe = new Vector2(-1, 1);
        }
        else
        {
            direccionGolpe = new Vector2(1, 1);
        }

        rbody.AddForce(direccionGolpe * fuezaGolpe);

        if (rbody.velocity.y > maxVelYDespuesDelGolpe)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, maxVelYDespuesDelGolpe);
        }

        // Solo una corrutina que controla todo el proceso
        StopAllCoroutines();
        StartCoroutine(ProcesarGolpe());
        EsperarYActivarMovimiento();
    }

    IEnumerator ProcesarGolpe()
    {
        // Mostrar sprite golpeado por un tiempo fijo
        yield return new WaitForSeconds(duracionGolpe);

        // Esperar a que toque el suelo
        while (!EstaEnSuelo())
        {
            yield return null;
        }

        // Restaurar control y animaciones
        puedeMoverse = true;
        spr.sprite = null;
        anim.enabled = true;
    }

public IEnumerator CambiarSpritePorGolpe()
{
    anim.enabled = false; // Desactiva el animator para mostrar solo el sprite golpeado
    spr.sprite = spriteGolpeado;

    yield return new WaitForSeconds(duracionGolpe);

    anim.enabled = true; // Reactiva animaciones normales
    spr.sprite = null;   // Deja que el animator recupere el sprite según el estado
}
IEnumerator EsperarYActivarMovimiento()
  {
      yield return new WaitForSeconds(0.1f);
      while (!EstaEnSuelo())
      {
          yield return null;
      }

      puedeMoverse = true;

      // Restaurar animaciones normales
      spr.sprite = null; // Deja que el Animator controle el sprite
      anim.enabled = true;
  }



}