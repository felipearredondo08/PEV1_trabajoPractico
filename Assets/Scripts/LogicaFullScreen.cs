using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LogicaFullScreen : MonoBehaviour
{

    public Toggle toggle;

    public TMP_Dropdown resolucionesDropDown;
    Resolution[] resoluciones;

    // Start is called before the first frame update
    void Start()
    {

        if (Screen.fullScreen){
            toggle.isOn = true ;
        }
        else
        {
            toggle.isOn = false;
        }

        RevisarResolucion();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ActivarPantallaCompleta (bool pantallaCompleta){

        Screen.fullScreen = pantallaCompleta;
    }

    public void RevisarResolucion(){
        resoluciones = Screen.resolutions;
        resolucionesDropDown.ClearOptions();
        List<String> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if(Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width && resoluciones[i].height == Screen.currentResolution.height){
                resolucionActual = i;
            }
        }
            resolucionesDropDown.AddOptions(opciones);
            resolucionesDropDown.value = resolucionActual;
            resolucionesDropDown.RefreshShownValue();

      }

      public void CambiarResolucion(int indiceResolucion){

        Resolution resolucion = resoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
      }
}

