using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VidaJugador : MonoBehaviour
{
    [SerializeField]
    GameObject[] corazones;
    public static int vidas = 3;
    bool coolDown = true;
    // Start is called before the first frame update
    void Start()
    {
        GameObject contenedorCorazones = GameObject.FindGameObjectWithTag("contadorCorazones");

        int childCount = contenedorCorazones.transform.childCount;

        corazones = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            corazones[i] = contenedorCorazones.transform.GetChild(i).gameObject;
            corazones[i].SetActive(false);
        }

        actualizarCorazones();

    }

    void actualizarCorazones()
    {
        

        for (int j = 0; j < vidas; j++)
        {
            corazones[j].SetActive(true);
        }
    }

    public void restarVida()
    {

        if (vidas > 0 && coolDown)
        {
            vidas--;
            corazones[vidas].SetActive(false);
            coolDown = false;
            Invoke("tiempoCoolDown", 0.5f);
        }
    }

    void tiempoCoolDown()
    {
        coolDown = true;
    }
}
    // Update is called once per frame
   
