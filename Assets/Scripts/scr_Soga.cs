using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Soga : MonoBehaviour
{

    public Vector3 fuerzaPrueba; 


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        transform.GetChild(transform.childCount - 1).GetComponent<Rigidbody>().AddForce(fuerzaPrueba, ForceMode.Impulse);

         if(Input.GetKeyDown(KeyCode.O))
        transform.GetChild(transform.childCount /2 ).GetComponent<Rigidbody>().AddForce(fuerzaPrueba, ForceMode.Impulse);
    }
}
