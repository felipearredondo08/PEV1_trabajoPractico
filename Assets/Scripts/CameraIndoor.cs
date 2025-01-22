using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIndoor : MonoBehaviour
{
   

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
                
              

                // Actualiza la posición anterior del jugador
                previousPlayerPosition = currentPlayerPosition;
            }
        }
    }
}