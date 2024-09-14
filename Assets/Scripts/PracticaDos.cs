using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //Aqui tuve que agregar la "libreria" System de C# porque sino no funcionaban las "funciones" Math.

public class PracticaDos : MonoBehaviour

{

/////////////////////////////////////////////////EJERCICIO N°1/////////////////////////////////////////////////////////////

//inicializamos variables en 0
    int mayor = 0;
    int menor = 0;
    int promedio = 0;
public string dentroDeRango(int n1, int n2, int n3) //Aqui determinamos que la funcion va a recibir 3 parametros.
 {
       
        mayor = Math.Max(n1, Math.Max(n2, n3)); //guarda el numero mayor de los 3 en la variable mayor.

        menor = Math.Min(n1, Math.Min(n2, n3)); //guarda en numero menor de los 3 en la variable menor.

       //condicionales para saber si el numero esta fuera de rango o dentro.
       if (mayor > 100)
        {
            return "Mayor fuera de rango";
        }
        else if (menor < 0)
        {
            return "Menor fuera de rango";
        }
        else
        {
           
            promedio = (n1 + n2 + n3) / 3; //realiza el promedio de los 3 numeros.
            return $"El valor promedio es: {promedio}"; 
        }

 }
 
 
 
 
 

    // Start is called before the first frame update
    void Start()
    {
             //////////////////////////////////////////////EJERCICIO N°2////////////////////////////////////////////////////

        int n1 = UnityEngine.Random.Range(-100, 100); //unity genera 3 numeros aleatorios entre -100 y 100 y los guarda en 3 variables.
        
        int n2 = UnityEngine.Random.Range(-100, 100);
        
        int n3 = UnityEngine.Random.Range(-100, 100);

           string resultado = dentroDeRango(n1, n2, n3); //aqui ejecuta la funcion y devuelve el string correspondiente a los condiciones if de la funcion

        
        Debug.Log($"Valores generados: {n1}, {n2}, {n3}"); //aqui muestra los valores generados aleatoriamente por consola.
        
        Debug.Log(resultado); //aqui muestra el string almacenado en la variable resultado por consola.


      
    }


      
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

