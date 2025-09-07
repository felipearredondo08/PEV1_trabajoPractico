using UnityEngine;
using UnityEngine.UI;

public class OffscreenIndicator : MonoBehaviour
{
    [Header("Scene refs")]
    public Transform target;             // tornado a seguir
    public Transform player;             // el jugador (para filtrar por distancia)
    public UnityEngine.Camera mainCam;               // cámara de juego
    public Canvas canvas;                // canvas donde vive este indicador

    [Header("UI refs")]
    public RectTransform root;           // RectTransform del GameObject indicador
    public Image icon;                   // ícono del tornado
    public Image arrow;                  // flecha que apunta

    [Header("Rules")]
    public float maxDistance = 60f;      // no mostrar si está más lejos que esto
    public float minDistance = 5f;       // opcional: no mostrar si está pegado al player
    public float screenPadding = 24f;    // margen interno de pantalla
    public bool hideWhenOnScreen = true; // ocultar si el tornado es visible
    [Header("UI Offset")]
    public float edgeOffset = 50f; // px desde el borde
    // cache
    RectTransform _canvasRect;
    CanvasGroup _group;


    void EnsureInit()
    {
        if (_group == null) _group = GetComponent<CanvasGroup>();
        if (_canvasRect == null && canvas != null)
            _canvasRect = canvas.GetComponent<RectTransform>();
    }
    void LateUpdate()
    {
        EnsureInit();

        if (target == null || player == null || mainCam == null || canvas == null || _canvasRect == null)
        {
            SetVisible(false);
            return;
        }

        // --- 1) Proyección a pantalla ---
        Vector3 sp = mainCam.WorldToScreenPoint(target.position);

        // --- 2) ¿Está dentro del viewport? ---
        bool onScreen =
            sp.z > 0f &&
            sp.x >= 0f && sp.x <= Screen.width &&
            sp.y >= 0f && sp.y <= Screen.height;

        // --- 3) Corte por distancia máxima (solo para objetos muy lejanos) ---
        float d = Vector3.Distance(player.position, target.position);
        bool tooFar = d > maxDistance;

        // Mostrar SOLO si está fuera de pantalla y no muy lejos
        if ((hideWhenOnScreen && onScreen) || tooFar)
        {
            SetVisible(false);
            return;
        }

        // Si está detrás de la cámara, trátalo como off-screen
        if (sp.z < 0f)
        {
            sp.x = Screen.width - sp.x;
            sp.y = Screen.height - sp.y;
            sp.z = 0f;
        }

        // --- 4) Clamping a los bordes con padding + edgeOffset ---
        float left = screenPadding + edgeOffset;
        float right = Screen.width - screenPadding - edgeOffset;
        float bottom = screenPadding + edgeOffset;
        float top = Screen.height - screenPadding - edgeOffset;

        Vector2 clamped = new Vector2(
            Mathf.Clamp(sp.x, left, right),
            Mathf.Clamp(sp.y, bottom, top)
        );

        // --- 5) Conversión a coordenadas locales del Canvas ---
        var uiCam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : mainCam;

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect, clamped, uiCam, out uiPos);
        root.anchoredPosition = uiPos;

        // --- 6) Rotación de la flecha apuntando al objetivo ---
        Vector2 uiRaw;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect, new Vector2(sp.x, sp.y), uiCam, out uiRaw);

        Vector2 dir = (uiRaw - uiPos);
        if (dir.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrow.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        // --- 7) Mostrar ---
        icon.enabled = true;
        arrow.enabled = true;
        SetVisible(true);
    }

    void SetVisible(bool v)
    {
        if (_group != null)
        {
            _group.alpha = v ? 1f : 0f;
            _group.interactable = v;
            _group.blocksRaycasts = v;
        }
        else
        {
            gameObject.SetActive(v);
        }
    }
}
