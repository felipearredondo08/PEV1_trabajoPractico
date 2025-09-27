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


    // Rope "coyote time" (ventana para agarrar cuerda)
    [SerializeField] private float ropeCoyoteTime = 0.15f;
    private float ropeCoyoteCounter = 0f;
    private Collider2D ropeCandidate = null;

    // Ajustes arriba en la clase (puedes dejar estos valores públicos)
    [SerializeField] private float joyDeadzone = 0.12f;  // más chico para que el stick responda
    [SerializeField] private float kbDeadzone = 0.01f;  // casi nulo, por si usás A/D o flechas
    [SerializeField] private float ropeMaxSpeed = 4.5f;
    [SerializeField] private float ropeAccel = 25f;
    private float ropeVelTarget = 0f;
    private Vector3 baseScale;
    //Mecanica de balanceo
    // Salto desde soga: intención pegajosa + impulso horizontal
    [SerializeField] private float ropeIntentBufferTime = 0.25f; // ventana para recordar dirección
    [SerializeField] private float ropeJumpHorizSpeed = 5.5f;   // impulso horizontal al saltar
    private float lastRopeDir = 0f;          // -1 izquierda, +1 derecha
    private float lastRopeDirTimer = 0f;     // cuenta regresiva del buffer
    // Salto asistido desde soga
    [Header("Asistencia al salto desde soga")]
    [SerializeField] private float ropeJumpUpSpeed = 7.0f;   // impulso vertical base
    [SerializeField] private float ropeAssistSearchRadius = 12f; // radio para buscar próxima soga
    [SerializeField] private float ropeAssistDYThreshold = 0.5f; // cuánto arriba debe estar para asistir
    [SerializeField] private float ropeAssistPerMeter = 1.5f; // +VY por metro de diferencia
    [SerializeField] private float ropeAssistVYMax = 3.5f; // tope del extra vertical

    // Amortiguación breve de la soga al agarrar
    [SerializeField] private float ropeGrabDampenTime = 0.15f;
    [SerializeField] private float ropeGrabExtraDrag = 4f;
    [SerializeField] private float ropeGrabExtraAngularDrag = 1.5f;


    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>(); // Inicializar el SpriteRenderer
        baseScale = transform.localScale; // <— importante para que no “se encoja”
    }

    void Awake()
    {
        instance = this;
    }



    void Update()
    {
        // Combina teclado (Horizontal/Vertical) + joystick (HorizontalJoy/VerticalJoy)
        float movX = GetAxisCombined("Horizontal", "HorizontalJoy", true);
        float movY = GetAxisCombined("Vertical", "VerticalJoy", true);

        // --- Estado agarrado: manejar y salir del frame ---
        if (agarrado)
        {
            HandleWhileGrabbed(movX, movY);
            return; // no mezclar con movimiento normal ni flip
        }

        // --- Movimiento normal / flip (solo si NO está agarrado) ---
        flip(movX);
        Walk(movX); // internamente respeta puedeMoverse

        // --- Suelo & Coyote Time ---
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer);

        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        // --- Saltar ---
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // --- Ventana para agarrar cuerda (rope coyote) ---
        if (ropeCoyoteCounter > 0f) ropeCoyoteCounter -= Time.deltaTime;

        bool grabPressed = Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire3");
        if (!agarrado && ropeCoyoteCounter > 0f && ropeCandidate != null && grabPressed)
        {
            TryGrabRope(ropeCandidate);
        }

        // --- Animator flags base ---
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);

        // --- Caída libre (sprite de caída y pausa de anim mientras cae) ---
        if (rbody.velocity.y < 0 && !isGrounded)
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
                    spr.sprite = fallSprite;
                    anim.enabled = false;
                }
            }
        }
        else if (isFalling)
        {
            isFalling = false;
            anim.enabled = true;
            spr.sprite = null;
        }

        if (!puedeMoverse) return;
    }


    void FixedUpdate()
    {
        if (agarrado && tramoAgarrado != null)
        {
            var ropeRb = tramoAgarrado.GetComponent<Rigidbody2D>();
            if (ropeRb != null)
            {
                float newVX = Mathf.MoveTowards(ropeRb.velocity.x, ropeVelTarget, ropeAccel * Time.fixedDeltaTime);
                ropeRb.velocity = new Vector2(newVX, ropeRb.velocity.y);
            }
        }
    }
    private float GetAxisCombined(string axisKb, string axisJoy, bool raw = true)
    {
        float a = raw ? Input.GetAxisRaw(axisKb) : Input.GetAxis(axisKb);
        float b = raw ? Input.GetAxisRaw(axisJoy) : Input.GetAxis(axisJoy);
        return Mathf.Abs(b) > Mathf.Abs(a) ? b : a; // el de mayor magnitud manda
    }
    private void HandleWhileGrabbed(float movX, float movY)
    {
        // Mantener escala base (evita encogerse por flips previos)
        float signX = Mathf.Sign(Mathf.Abs(transform.localScale.x) < 0.0001f ? 1f : transform.localScale.x);
        transform.localScale = new Vector3(signX * Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);

        // Seguir tramo (posición + rotación)
        if (tramoAgarrado != null)
            transform.SetPositionAndRotation(tramoAgarrado.position + offset, tramoAgarrado.rotation);

        // --- Lectura de intención lateral: priorizar joystick, luego teclado ---
        float joy = Input.GetAxisRaw("HorizontalJoy"); // joystick
        float kb = Input.GetAxisRaw("Horizontal");    // teclado
        float raw;
        bool usingJoy = Mathf.Abs(joy) > joyDeadzone;

        if (usingJoy)
        {
            float s = Mathf.Sign(joy);
            float t = Mathf.InverseLerp(joyDeadzone, 1f, Mathf.Abs(joy)); // 0..1
            raw = s * (t * t); // curva suave
        }
        else
        {
            raw = Mathf.Abs(kb) > kbDeadzone ? Mathf.Sign(kb) : 0f; // teclado digital
        }

        // Sprite de balanceo (intención visual)
        if (movX > 0.01f) spr.sprite = spriteBalanceoDerecha;
        else if (movX < -0.01f) spr.sprite = spriteBalanceoIzquierda;
        else if (Mathf.Abs(raw) > 0) spr.sprite = raw > 0 ? spriteBalanceoDerecha : spriteBalanceoIzquierda;

        // --- Sticky intent: recordar última dirección válida un rato ---
        if (Mathf.Abs(raw) > 0.01f)
        {
            lastRopeDir = Mathf.Sign(raw);
            lastRopeDirTimer = ropeIntentBufferTime;
        }
        else if (lastRopeDirTimer > 0f)
        {
            lastRopeDirTimer -= Time.deltaTime;
        }

        // --- Soltar / saltar desde soga ---
        if (Input.GetButtonDown("Jump"))
        {
            if (movY < 0) // caída deliberada
            {
                seSuelta();
                return;
            }

            // 1) dirección preferida para el salto
            float dir = 0f;
            if (Mathf.Abs(raw) > 0.01f) dir = Mathf.Sign(raw);
            else if (lastRopeDirTimer > 0f) dir = lastRopeDir;
            else
            {
                var ropeRb = tramoAgarrado ? tramoAgarrado.GetComponent<Rigidbody2D>() : null;
                if (ropeRb && Mathf.Abs(ropeRb.velocity.x) > 0.05f) dir = Mathf.Sign(ropeRb.velocity.x);
            }
            if (Mathf.Abs(dir) < 0.01f) dir = 1f; // fallback a la derecha

            // Asegurar que Jump() se ejecute: renovar coyote antes de soltar
            coyoteTimeCounter = coyoteTime;

            // Soltar (pasa a dinámico) + salto estándar (audio/anim/vertical base)
            seSueltaYSalta();
            Jump();

            // Base: impulso horizontal y conservar Y de Jump()
            float targetVX = ropeJumpHorizSpeed * dir;
            float targetVY = rbody.velocity.y;

            // Assist vertical si la próxima cuerda está más alta
            Transform nextRope = FindNextRopeAhead(dir, ropeAssistSearchRadius);
            if (nextRope != null)
            {
                float dy = nextRope.position.y - transform.position.y;
                if (dy > ropeAssistDYThreshold)
                {
                    float extra = Mathf.Min(dy * ropeAssistPerMeter, ropeAssistVYMax);
                    targetVY += extra;
                }
            }

            // Aplicar velocidad final
            rbody.velocity = new Vector2(targetVX, targetVY);
            return; // terminar este frame aquí
        }

        // --- Objetivo de velocidad lateral de la cuerda (aplicado en FixedUpdate) ---
        ropeVelTarget = raw * ropeMaxSpeed;
    }
    private Transform FindNextRopeAhead(float dir, float radius)
    {
        var cols = Physics2D.OverlapCircleAll(transform.position, radius);
        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (var c in cols)
        {
            if (!c || !c.CompareTag("Cuerda")) continue;
            Vector2 to = c.transform.position - transform.position;
            if (Mathf.Sign(to.x) != Mathf.Sign(dir)) continue; // solo las de adelante
            float d = to.sqrMagnitude;
            if (d < bestDist) { bestDist = d; best = c.transform; }
        }
        return best;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cuerda"))
        {
            ropeCandidate = other;
            ropeCoyoteCounter = ropeCoyoteTime; // abre la ventana para agarrar
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == ropeCandidate && !agarrado)
        {
            ropeCandidate = null;
            ropeCoyoteCounter = 0f;
        }
    }
    private void TryGrabRope(Collider2D ropeCol)
    {
        if (ropeCol == null) return;
        Grab(ropeCol);
        // Al agarrar, ya no necesitamos la ventana
        ropeCoyoteCounter = 0f;
    }

    private void Grab(Collider2D ropeCol)
    {
        agarrado = true;

        // Elegir un tramo estable (evita el último eslabón/leaf)
        Transform stable = ChooseStableSegment(ropeCol.transform);
        tramoAgarrado = stable != null ? stable : ropeCol.transform;

        // Resetear toda la cadena (sin heredar inercia)
        ResetRopeChain(tramoAgarrado.GetComponent<Collider2D>() ?? ropeCol);

        // Amortiguar la cadena un instante (drag alto temporal)
        StartCoroutine(DampenRopeOnGrab(tramoAgarrado, ropeGrabDampenTime));

        // Resetear intención/objetivo de balanceo
        ropeVelTarget = 0f;
        lastRopeDir = 0f;
        lastRopeDirTimer = 0f;

        // Congelar al player y limpiar su velocidad
        rbody.isKinematic = true;
        rbody.velocity = Vector2.zero;

        // Desactivar animaciones normales
        anim.enabled = false;
    }
    // Si el segmento tocado es "leaf" (nadie lo tiene como connectedBody),
    // usamos su eslabón superior (el connectedBody del propio Hinge).
    private Transform ChooseStableSegment(Transform hit)
    {
        if (hit == null) return null;

        var rb = hit.GetComponent<Rigidbody2D>();
        if (rb == null) return hit;

        // ¿Algún joint en el grupo cuelga de este rb? (si sí, NO es leaf)
        Transform group = hit.parent != null ? hit.parent : hit;
        var jointsInGroup = group.GetComponentsInChildren<HingeJoint2D>(true);

        bool hasChildAttached = false;
        foreach (var hj in jointsInGroup)
        {
            if (hj != null && hj.connectedBody == rb) { hasChildAttached = true; break; }
        }

        if (!hasChildAttached)
        {
            // Es leaf: buscamos su eslabón superior
            var myHinge = hit.GetComponent<HingeJoint2D>();
            if (myHinge != null && myHinge.connectedBody != null)
                return myHinge.connectedBody.transform;
        }
        return hit;
    }

    // Subir drag/angDrag un ratito para apagar latigazos iniciales
    private IEnumerator DampenRopeOnGrab(Transform anySegment, float time)
    {
        if (anySegment == null) yield break;

        Transform group = anySegment.parent != null ? anySegment.parent : anySegment;
        var rbs = group.GetComponentsInChildren<Rigidbody2D>(true);

        // Guardar y aplicar
        var originals = new List<(Rigidbody2D rb, float drag, float ang)>(rbs.Length);
        foreach (var rb in rbs)
        {
            if (rb == null) continue;
            originals.Add((rb, rb.drag, rb.angularDrag));

            // Reiniciar velocidades por si algo quedó oscilando
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.drag = rb.drag + ropeGrabExtraDrag;
            rb.angularDrag = rb.angularDrag + ropeGrabExtraAngularDrag;
        }

        yield return new WaitForSeconds(time);

        // Restaurar valores originales
        foreach (var o in originals)
        {
            if (o.rb == null) continue;
            o.rb.drag = o.drag;
            o.rb.angularDrag = o.ang;
        }
    }

    private void ResetRopeChain(Collider2D ropeCol)
    {
        var startRB = ropeCol.attachedRigidbody ?? ropeCol.GetComponentInParent<Rigidbody2D>();
        if (startRB == null) return;

        // Buscamos todos los RB conectados por HingeJoint2D (en ambos sentidos)
        var visited = new HashSet<Rigidbody2D>();
        var stack = new Stack<Rigidbody2D>();
        stack.Push(startRB);

        // Para no escanear toda la escena, limitamos joints al mismo "grupo" (padre)
        Transform group = startRB.transform.parent != null ? startRB.transform.parent : startRB.transform;
        var jointsInGroup = group.GetComponentsInChildren<HingeJoint2D>(true);

        while (stack.Count > 0)
        {
            var rb = stack.Pop();
            if (rb == null || !visited.Add(rb)) continue;

            // Apagamos su movimiento
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            // rb.Sleep(); // opcional: dormir el RB para que quede planchado

            // 1) Joints pegados a este RB
            foreach (var hj in rb.GetComponents<HingeJoint2D>())
            {
                if (hj.connectedBody != null && !visited.Contains(hj.connectedBody))
                    stack.Push(hj.connectedBody);
            }
            // 2) Joints de otros que lo tienen como connectedBody
            foreach (var hj in jointsInGroup)
            {
                if (hj != null && hj.connectedBody == rb && hj.attachedRigidbody != null && !visited.Contains(hj.attachedRigidbody))
                    stack.Push(hj.attachedRigidbody);
            }
        }
    }

    /*  public void Walk()
      {
          if (!puedeMoverse) return;

          float horizontalInput = Input.GetAxis("Horizontal");
          rbody.velocity = new Vector2(horizontalInput * movementSpeed, rbody.velocity.y);
          isMoving = horizontalInput != 0;
      }
    */
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

    public void Walk(float movX)
    {
        if (!puedeMoverse) return;
        rbody.velocity = new Vector2(movX * movementSpeed, rbody.velocity.y);
        isMoving = Mathf.Abs(movX) > 0.01f;
    }

    public void flip(float movHor)
    {
        if (movHor > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movHor < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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

        anim.SetTrigger("Damage");
        //audioSource.PlayOneShot

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
    void OnDrawGizmosSelected()
    {
        // color verde si detecta suelo, rojo si no
        Gizmos.color = Color.red;
#if UNITY_EDITOR
        if (Application.isPlaying && isGrounded)
            Gizmos.color = Color.green;
#endif

        // Dibuja el círculo del check
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundRayDist, radius);
    }


}