using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{



    public float movementSpeed = 100; //definimos una variable para la velocidad del movimiento

    public float jumpForce = 100f; //definimos una variable para luego multiplicar la fuerza de salto.

    private Rigidbody2D rbody; // definimos la variable donde se guardara el componente Rigidbody2d de unity

    public bool isGrounded = false; //creamos un booleano donde decimos que esta tocando el suelo es falso

    public bool isMoving = false;

    public float groundRayDist = 1.5f;//creamos una variable para multiplicar el largo del rayo que nos indicara si es suelo o no.

    public LayerMask groundLayer;//definimos una variable donde guardar el nombre de la etiqueta de "suelo".

    public float radius = 1.3f;//creamos una variable para achicar o contraer el radio del circulo que hara contacto con el suelo.

    private Animator anim;

    private SpriteRenderer spr;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///WIND CONTROLLER -----> preguntar <summary>
    /// WIND CONTROLLER -----> preguntar
    public Vector2 windDirection; // Dirección actual del viento
    public float windForce = 0f; // Fuerza actual del viento
    public float minWindForce = 5000f; // Fuerza mínima del viento
    public float maxWindForce = 10000f; // Fuerza máxima del viento
    public float windChangeInterval = 3f; // Intervalo de tiempo para cambiar el viento
    private float windChangeTimer = 0f; // Temporizador para cambiar el viento        



    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //traemos el componente Rigidbody2d y lo guardamos en la variable que habiamos creado.
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    /**void Update()
    {



        flip();



        //rbody.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rbody.velocity.y); //le pasamos como parametro a la propiedad velocity del componente Rigidbody2d un vector2, lo multiplicamos por la variable movementSpeed y le asignamos las teclas a utilizar.

        Walk();

        isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer); //con vector3.down indicamos que el rayo con forma circular debe ir hacia abajo para ver si hay suelo.


        if (Input.GetKeyDown(KeyCode.Space)) //determinamos que con la tecla space se ejecutara la funcion jump.
        {
            Jump();
        }

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);


    } **/

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
        if (horizontalInput != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

    }

   /** public void Jump()
    {
        if (!isGrounded) return; //con este condicional decimos que si isGrounded no se cumple entonces se podra saltar.

        rbody.velocity = Vector2.up * jumpForce; // utilizamos vector2.up para que se genere una velocidad hacia arriba en el transform y la multiplicamos por jumpForce.



    }**/

    public void Jump()
{
    if (!isGrounded) return; // Solo salta si está en el suelo

    // Aplica la fuerza de salto
    rbody.velocity = Vector2.up * jumpForce;

    // Aplica la fuerza del viento
    rbody.AddForce(windDirection * windForce, ForceMode2D.Impulse);
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
        windDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;

        // Cambia la fuerza del viento en un rango
       windForce = UnityEngine.Random.Range(minWindForce, maxWindForce);

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
}

