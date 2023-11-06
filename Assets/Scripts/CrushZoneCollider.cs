using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrushZoneCollider : MonoBehaviour
{
    AudioSource klaxonCrush;
    public TextMeshProUGUI crushZoneText;
    // Start is called before the first frame update
    void Start()
    {
        klaxonCrush = GetComponent<AudioSource>();
        crushZoneText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        klaxonCrush.Play();
        crushZoneText.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        klaxonCrush.Stop();
        crushZoneText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
