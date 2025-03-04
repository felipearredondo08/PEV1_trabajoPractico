using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soga2d : MonoBehaviour
{

    public Vector2 fuerzaPrueba;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
 void Update()
{
    if (Input.GetKeyDown(KeyCode.P))
    {
        if (transform.childCount > 0) // Verifica que haya al menos un hijo
        {
            transform.GetChild(transform.childCount - 1).GetComponent<Rigidbody2D>().AddForce(fuerzaPrueba, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("No hay hijos en el objeto, no se puede aplicar la fuerza.");
        }
    }
}

}
