using UnityEngine;
using System.Collections;

public class ChifleteToggle : MonoBehaviour
{
    public GameObject chiflete; // Asigna el GameObject "Chiflete" desde el inspector
    public float minActiveTime = 5f; // Tiempo mínimo que debe estar activo
    public float minInactiveTime = 1f; // Tiempo mínimo que debe estar inactivo
    public float maxInactiveTime = 5f; // Tiempo máximo que debe estar inactivo

    private ParticleSystem particleSystemChiflete; // Referencia al ParticleSystem de Chiflete

    private void Start()
    {
        // Obtiene el componente ParticleSystem del GameObject Chiflete
        particleSystemChiflete = chiflete.GetComponent<ParticleSystem>();

        // Inicia el ciclo de encendido y apagado
        StartCoroutine(ToggleChiflete());
    }

    private IEnumerator ToggleChiflete()
    {
        while (true)
        {
            // Activa "Chiflete" y todos sus hijos
            SetChifleteActive(true);

            // Mantén "Chiflete" activo por al menos el tiempo mínimo
            yield return new WaitForSeconds(minActiveTime);

            // Apaga "Chiflete" y todos sus hijos
            SetChifleteActive(false);

            // Espera un tiempo aleatorio antes de volver a encenderlo
            float inactiveTime = Random.Range(minInactiveTime, maxInactiveTime);
            yield return new WaitForSeconds(inactiveTime);
        }
    }

    private void SetChifleteActive(bool state)
    {
        // Activa o desactiva todos los hijos de "Chiflete"
        foreach (Transform child in chiflete.transform)
        {
            child.gameObject.SetActive(state);
        }

        // Activa o desactiva el ParticleSystem del propio Chiflete
        if (particleSystemChiflete != null)
        {
            if (state)
            {
                particleSystemChiflete.Play(); // Activa el ParticleSystem
            }
            else
            {
                particleSystemChiflete.Stop(); // Detiene el ParticleSystem
            }
        }
    }
}
