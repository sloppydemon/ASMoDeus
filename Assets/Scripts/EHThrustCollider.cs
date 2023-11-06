using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EHThrustWarn : MonoBehaviour
{
    AudioSource klaxonThrust;
    // Start is called before the first frame update
    void Start()
    {
        klaxonThrust = GetComponent<AudioSource>();
        //klaxonThrust = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            klaxonThrust.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            klaxonThrust.Stop();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
