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

    public float groundRayDist = 1.5f;//creamos una variable para multiplicar el largo del rayo que nos indicara si es suelo o no.

    public LayerMask groundLayer;//definimos una variable donde guardar el nombre de la etiqueta de "suelo".

    public float radius = 1.3f;//creamos una variable para achicar o contraer el radio del circulo que hara contacto con el suelo.

 

    // Start is called before the first frame update
    void Start()
    {
         rbody = GetComponent<Rigidbody2D>(); //traemos el componente Rigidbody2d y lo guardamos en la variable que habiamos creado.
    }

    // Update is called once per frame
    void Update()
    {
          rbody.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rbody.velocity.y); //le pasamos como parametro a la propiedad velocity del componente Rigidbody2d un vector2, lo multiplicamos por la variable movementSpeed y le asignamos las teclas a utilizar.

         isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer); //con vector3.down indicamos que el rayo con forma circular debe ir hacia abajo para ver si hay suelo.

         if (Input.GetKeyDown(KeyCode.Space)) //determinamos que con la tecla space se ejecutara la funcion jump.
        {
            Jump();
        }

        
    }

    public void Jump(){
        if (!isGrounded) return; //con este condicional decimos que si isGrounded no se cumple entonces se podra saltar.

        rbody.velocity = Vector2.up * jumpForce; // utilizamos vector2.up para que se genere una velocidad hacia arriba en el transform y la multiplicamos por jumpForce.
        

    }




}

