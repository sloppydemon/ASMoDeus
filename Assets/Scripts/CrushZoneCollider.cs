using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrushZoneCollider : MonoBehaviour
{
    AudioSource klaxonCrush;
    
    // Start is called before the first frame update
    void Start()
    {
        klaxonCrush = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        klaxonCrush.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        klaxonCrush.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
