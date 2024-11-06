/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar y reiniciar escenas

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reinicia la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
} */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public AudioSource deathSound; // Asigna aquí el AudioSource que contiene el sonido

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Inicia la corrutina para ejecutar el sonido, esperar, y luego cargar la escena "gameover"
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        // Reproduce el sonido de muerte
        if (deathSound != null)
        {
            deathSound.Play();
        }

        // Espera 3 segundos
        yield return new WaitForSeconds(0.1f);

        // Guarda el índice de la escena actual y cambia a "gameover"
        Pantallagameover.SetLastLevelIndex(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("gameover");
    }
}