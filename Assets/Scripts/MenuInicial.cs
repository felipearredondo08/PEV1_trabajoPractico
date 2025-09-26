/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void jugar(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void salir(){
        Application.Quit();
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    void Update()
    {
        // Detectar si el jugador presiona la barra espaciadora o el botón de salto del joystick
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            jugar();
        }

        // También puedes salir del juego con Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            salir();
        }
    }

    public void jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void salir()
    {
        Application.Quit();
    }
}
