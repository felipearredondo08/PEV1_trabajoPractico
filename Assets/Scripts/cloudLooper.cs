using UnityEngine;

public class cloudLooper : MonoBehaviour
{
    [Header("Bordes (world space)")]
    public Transform leftEdge;
    public Transform rightEdge;

    [Header("Movimiento")]
    public bool moveRight = true;
    public Vector2 speedRange = new Vector2(0.6f, 1.2f);
    public float parallaxFactor = 1f;
    public float margin = 2f;

    [Header("Altura")]
    [Tooltip("Usa la Y actual como línea base (recomendado).")]
    public bool useCurrentYAsBase = true;
    [Tooltip("Si desactivas 'useCurrentYAsBase', se usará este valor de Y en mundo.")]
    public float fixedBaseY = 0f;
    [Tooltip("Desviación vertical aleatoria respecto a la base.")]
    public float yJitter = 0f;   // pon 0 para que no se mueva en Y

    [Header("Apariencia opcional")]
    public bool randomizeScaleOnRespawn = true;
    public Vector2 scaleRange = new Vector2(0.9f, 1.1f);

    float baseY;
    float speed;

    void Awake()
    {
        baseY = useCurrentYAsBase ? transform.position.y : fixedBaseY;
        speed = Random.Range(speedRange.x, speedRange.y);

        if (randomizeScaleOnRespawn)
        {
            float s = Random.Range(scaleRange.x, scaleRange.y);
            transform.localScale = new Vector3(s, s, 1f);
        }
    }

    void Update()
    {
        float dir = moveRight ? 1f : -1f;
        transform.Translate(Vector3.right * dir * speed * parallaxFactor * Time.deltaTime, Space.World);

        if (moveRight)
        {
            if (transform.position.x > rightEdge.position.x + margin)
                WrapToLeft();
        }
        else
        {
            if (transform.position.x < leftEdge.position.x - margin)
                WrapToRight();
        }
    }

    void WrapToLeft()
    {
        Vector3 p = transform.position;
        p.x = leftEdge.position.x - margin;
        p.y = baseY + (yJitter > 0f ? Random.Range(-yJitter, yJitter) : 0f);
        transform.position = p;

        RespawnRandoms();
    }

    void WrapToRight()
    {
        Vector3 p = transform.position;
        p.x = rightEdge.position.x + margin;
        p.y = baseY + (yJitter > 0f ? Random.Range(-yJitter, yJitter) : 0f);
        transform.position = p;

        RespawnRandoms();
    }

    void RespawnRandoms()
    {
        speed = Random.Range(speedRange.x, speedRange.y);
        if (randomizeScaleOnRespawn)
        {
            float s = Random.Range(scaleRange.x, scaleRange.y);
            transform.localScale = new Vector3(s, s, 1f);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (leftEdge && rightEdge)
        {
            Gizmos.color = new Color(1, 1, 1, 0.5f);
            Gizmos.DrawLine(leftEdge.position + Vector3.down * 20f, leftEdge.position + Vector3.up * 20f);
            Gizmos.DrawLine(rightEdge.position + Vector3.down * 20f, rightEdge.position + Vector3.up * 20f);
        }
    }
#endif
}
