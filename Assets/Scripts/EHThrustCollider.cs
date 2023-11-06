using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EHThrustWarn : MonoBehaviour
{
    AudioSource klaxonThrust;
    public TextMeshProUGUI thrustEHText;
    // Start is called before the first frame update
    void Start()
    {
        klaxonThrust = GetComponent<AudioSource>();
        thrustEHText.enabled = false;
        //proximitySlider.colors.normalColor = new Color(0.14f, 1, 0, 1);
        //klaxonThrust = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            klaxonThrust.Play();
            thrustEHText.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            klaxonThrust.Stop();
            thrustEHText.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
