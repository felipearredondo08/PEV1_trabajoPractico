/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{


public GameObject door;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
        }

    }

  /*  private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            door.SetActive(true);
        }
    } */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    private bool playerInRange = false;

    public AudioSource audioSource;

void Start()
    {
       audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            door.SetActive(false);
            audioSource.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

   /*private void OnCollisionExit2D(Collision2D other)
    {
        if (playerInRange)
        {
            playerInRange = false;
            door.SetActive(true); // Opcional: Activa la puerta cuando el jugador salga del colisionador
        }
    }*/ 
    
}

