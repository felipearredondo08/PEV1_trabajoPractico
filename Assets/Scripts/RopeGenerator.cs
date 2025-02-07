using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject eslabonPrefab;
    [SerializeField] private int cantidadEslabones = 10;
    [SerializeField] private Transform puntoFijo; // Ahora esto es el techo fijo

    void Start()
    {
        GenerarCuerda();
    }

    void GenerarCuerda()
    {
        GameObject previoEslabon = null;

        for (int i = 0; i < cantidadEslabones; i++)
        {
            GameObject nuevoEslabon = Instantiate(eslabonPrefab, puntoFijo.position + Vector3.down * i * 0.1f, Quaternion.identity);
            Rigidbody2D rb = nuevoEslabon.GetComponent<Rigidbody2D>() ?? nuevoEslabon.AddComponent<Rigidbody2D>();
            rb.mass = 0.1f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            HingeJoint2D joint = nuevoEslabon.GetComponent<HingeJoint2D>() ?? nuevoEslabon.AddComponent<HingeJoint2D>();

            if (previoEslabon != null)
            {
                joint.connectedBody = previoEslabon.GetComponent<Rigidbody2D>();
            }
            else
            {
                // Conectar el primer eslabón al punto fijo
                joint.connectedBody = puntoFijo.GetComponent<Rigidbody2D>();
            }

            joint.anchor = new Vector2(0, -0.1f);
            joint.connectedAnchor = new Vector2(0, 0.1f);

            previoEslabon = nuevoEslabon;
        }
    }
}
