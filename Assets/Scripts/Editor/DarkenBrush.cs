using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DarkenBrush", menuName = "Brushes/Darken Brush")]
public class DarkenBrush : GridBrush
{
    [Range(0f, 1f)]
    public float darkenAmount = 0.1f; // Cantidad de oscuridad a aplicar (0 = sin cambio, 1 = completamente negro)

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        if (brushTarget == null)
            return;

        Tilemap tilemap = brushTarget.GetComponent<Tilemap>();

        if (tilemap != null)
        {
            TileBase tile = tilemap.GetTile(position);

            if (tile != null)
            {
                Color currentColor = tilemap.GetColor(position);
                Color darkenedColor = currentColor * (1f - darkenAmount); // Oscurece el color
                darkenedColor.a = currentColor.a; // Mantén la transparencia

                tilemap.SetColor(position, darkenedColor); // Aplica el color oscurecido
            }
        }
    }
}