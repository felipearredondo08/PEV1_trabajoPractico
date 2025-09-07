using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.R))
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
    }

    // Cerrar el juego al presionar 'Escape'
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        //Application.Quit(); // Cierra el juego (solo funciona en una build)
    }

    }

 
}
