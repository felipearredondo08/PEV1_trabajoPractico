using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
   public TMP_Text textoMonedas; // Asigna aquí el Text del contador de moneditas

    public void ActualizarContadorMoneditas(int cantidad)
    {
       // textoMonedas.text = "= " + cantidad;
       textoMonedas.text = "" + cantidad;
    }
}