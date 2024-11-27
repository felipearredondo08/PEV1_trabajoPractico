using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform bg1;
    public float factor1 = 1f;

    public Transform bg2;
    public float factor2 = 1 / 2f;

    public Transform bg3;
    public float factor3 = 1 / 4f;

    public Transform bg4;
    public float factor4 = 1 / 8f;

    public Transform bg5;
    public float factor5 = 1 / 16f;

    public Transform bg6;
    public float factor6 = 1 / 32f;

    private float iniCamPosFrame;
    private float nextCamPosFrame;

    // Variable para almacenar la posición anterior del jugador
    private Vector3 previousPlayerPosition;

    private void Start()
    {
        if (Controller.instance != null)
        {
            previousPlayerPosition = Controller.instance.transform.position;
        }
    }

    void Update()
    {
        if (Controller.instance != null)
        {
            iniCamPosFrame = transform.position.x;
            transform.position = new Vector3(Controller.instance.transform.position.x, transform.position.y, transform.position.z);
        }
    }

    void LateUpdate()
    {
        nextCamPosFrame = transform.position.x;

        // Calcula el desplazamiento solo si el jugador se ha movido
        if (Controller.instance != null)
        {
            Vector3 currentPlayerPosition = Controller.instance.transform.position;
            if (currentPlayerPosition != previousPlayerPosition)
            {
                float displacement = nextCamPosFrame - iniCamPosFrame;
                
                // Actualiza la posición de los fondos en función del desplazamiento
                bg1.position = new Vector3(bg1.position.x + displacement * factor1, bg1.position.y, bg1.position.z);
                bg2.position = new Vector3(bg2.position.x + displacement * factor2, bg2.position.y, bg2.position.z);
                bg3.position = new Vector3(bg3.position.x + displacement * factor3, bg3.position.y, bg3.position.z);
                bg4.position = new Vector3(bg4.position.x + displacement * factor4, bg4.position.y, bg4.position.z);
                bg5.position = new Vector3(bg5.position.x + displacement * factor5, bg5.position.y, bg5.position.z);
                bg6.position = new Vector3(bg6.position.x + displacement * factor6, bg6.position.y, bg6.position.z);

                // Actualiza la posición anterior del jugador
                previousPlayerPosition = currentPlayerPosition;
            }
        }
    }
}