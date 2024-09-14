using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting; //Aqui tuve que agregar la "libreria" System de C# porque sino no funcionaban las "funciones" Math.

public class PracticaDos : MonoBehaviour

{

    /////////////////////////////////////////////////EJERCICIO N°1/////////////////////////////////////////////////////////////

    //inicializamos variables en 0
    int mayor = 0;
    int menor = 0;
    int promedio = 0;

    int[] array = { 97, -64, -3, -58, -15, 58, 51, 38, -31, -37 }; //definimos los Numeros que tendra nuestro array y lo guardamos en la variable "array"

    float timer = 0f; //definimos una variable del tipo float para el timer porque necesitamos que cuente decimales ya que es 4,5 el tiempo de espera

    bool arraylisto=false; //utilizamos esto como bandera para que solo muestr el array corregido una sola vez utilizando un if como condicion. 

    public string DentroDeRango(int n1, int n2, int n3) //Aqui determinamos que la funcion va a recibir 3 parametros.
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

    public void Numeroparx2() //definimos la funcion como void porque no requerimos que retorne nada.
    {
        for (int i = 0; i < array.Length; i++) //el ciclo for recorrera el array y en vez de tener definido un numero para detenerse utiliza Lenght que mide el array automaticamente
        {
            if (array[i] % 2 == 0) //aqui en la condicion dice que si i(la posicion actual en el array) es par 
            {
                array[i] *= 2; // tomara ese numero y lo multiplicara por 2.
            }
        }

        Debug.Log("el array corregido es:");
        foreach (int n in array)
        {
            Debug.Log(n);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        //////////////////////////////////////////////EJERCICIO N°2////////////////////////////////////////////////////

        int n1 = UnityEngine.Random.Range(-100, 100); //unity genera 3 numeros aleatorios entre -100 y 100 y los guarda en 3 variables.

        int n2 = UnityEngine.Random.Range(-100, 100);

        int n3 = UnityEngine.Random.Range(-100, 100);

        string resultado = DentroDeRango(n1, n2, n3); //aqui ejecuta la funcion y devuelve el string correspondiente a las condiciones if de la funcion


        Debug.Log($"Valores generados: {n1}, {n2}, {n3}"); //aqui muestra los valores generados aleatoriamente por consola.

        Debug.Log(resultado); //aqui muestra el string almacenado en la variable resultado que estara cargado con el resultado de los IF de la funcion dentroDeRango por consola.



    }





    // Update is called once per frame
    void Update()
    {
        
       // timer += Time.deltaTime; //Carga la variable timer con el tiempo transcurrido no en frames sino corregido a deltatime.

        
        //if (timer >= 4.5f) // condicional que activa la funcion si el tiempo es mayor o igual a 4.5 segundos.
       // {

        //    Numeroparx2(); //llama a la funcion que duplicara los numeros pares.

            
          //  timer = 0f; //reinicia el timer a 0 para que no se quede ejecutando constantemente.
            
        //}

        if (!arraylisto)
        {
            timer += Time.deltaTime;

            // Verificar si han pasado 4.5 segundos
            if (timer >= 4.5f)
            {
               Numeroparx2(); //llama a la funcion que duplicara los numeros pares.

                
                arraylisto = true; //señal de que el if no se tiene que ejecutar mas. 
            }
        }

    }

}