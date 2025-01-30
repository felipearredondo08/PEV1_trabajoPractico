/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{

    //  public Animator animator;
 
    private Rigidbody2D rb;
    public float movHor = 0f;
    public float speed = 3f;

    public bool isGroundFloor = true ;
    public bool isGroundFront = false;

    public LayerMask groundLayer;
    public float frontGrndRayDist = 0.25f;
    public float floorCheckY = 0.52f;

    public float frontcheck = 0.51f;

    public float frontDist = 0.00f;

    public int scoreGive = 50;


    private RaycastHit2D hit;

    


    void Start()
    {
       rb = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {
        //para no caer por los bordes

        isGroundFloor = (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - floorCheckY, transform.position.z),
        new Vector3(movHor, 0, 0) , frontGrndRayDist, groundLayer));

        if (!isGroundFloor){
            movHor = movHor * -1;
        }

        //choque con pared

        if (Physics2D.Raycast(transform.position, new Vector3(movHor,0 , 0), frontcheck, groundLayer)){
            movHor = movHor * -1;
        }

        //choque con compañere

        hit = Physics2D.Raycast(new Vector3(transform.position.x + movHor*frontcheck, transform.position.y, transform.position.z),
        new Vector3(movHor,0,0), frontDist);

        if (hit != null){
             if (hit.transform != null)
                if (hit.transform.CompareTag("Enemy"))
                movHor = movHor *-1;
        }

    }

    void FixedUpdate() {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);
    }

    private void getKilled(){
        gameObject.SetActive(false);
    }


} */
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 2f;
    private int direction = 1; // 1 = Derecha, -1 = Izquierda

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    public float groundCheckOffsetX = 0.2f;
    public float frontRayDistance = 0.3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 🔹 Verificar si hay suelo adelante
        Vector3 groundRayOrigin = transform.position + Vector3.right * direction * groundCheckOffsetX;
        bool isGroundAhead = Physics2D.Raycast(groundRayOrigin, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(groundRayOrigin, Vector2.down * groundCheckDistance, Color.green);

        // 🔹 Verificar si hay un enemigo adelante con un RaycastAll para evitar bloqueos
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

        // 🔹 Si no hay suelo adelante o hay un enemigo, girar
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

        // 🔹 Empujar ligeramente en la nueva dirección para evitar bloqueo
        transform.position += new Vector3(direction * 0.03f, 0, 0);
    }
}
