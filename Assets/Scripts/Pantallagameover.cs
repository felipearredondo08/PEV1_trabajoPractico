using UnityEngine;
using UnityEngine.SceneManagement;

public class Pantallagameover : MonoBehaviour
{
    private static int lastLevelIndex; // Para almacenar el nivel en el que se perdió

    // Este método se llama desde DeathZone antes de cambiar a la escena "gameover"
    public static void SetLastLevelIndex(int index)
    {
        lastLevelIndex = index;
    }

    void Update()
    {
        // Reinicia el nivel anterior al presionar Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(lastLevelIndex);
        }
    }
}