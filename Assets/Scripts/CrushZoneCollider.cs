using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CrushZoneCollider : MonoBehaviour
{
    AudioSource klaxonCrush;
    public AudioSource klaxonEH;
    public AudioSource hullSounds;
    public TextMeshProUGUI crushZoneText;
    public TextMeshProUGUI EHText;
    public TextMeshProUGUI alertText;
    public Slider crushResistanceBar;
    public Image crushResistanceBarFill;
    public Slider proximityBar;
    public Color crushColorEmpty;
    public Color crushColorFull;
    Gravitation grav;
    Move move;
    // Start is called before the first frame update
    void Start()
    {
        klaxonCrush = GetComponent<AudioSource>();
        crushZoneText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            klaxonCrush.Play();
            hullSounds.Play();
            crushZoneText.enabled = true;
            grav = collision.gameObject.GetComponent<Gravitation>();
            move = collision.gameObject.GetComponent<Move>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            grav.crushResistance -= 0.01f / (proximityBar.value / proximityBar.maxValue);
            crushResistanceBar.value = grav.crushResistance;
            crushResistanceBarFill.color = Color.Lerp(crushColorEmpty, crushColorFull, crushResistanceBar.value / crushResistanceBar.maxValue);
            hullSounds.volume = 1f / (proximityBar.value / proximityBar.maxValue);
            hullSounds.pitch = 0.5f + (1.5f / (crushResistanceBar.value / crushResistanceBar.maxValue));
            if (grav.crushResistance <= 0f)
            {
                grav.transform.localScale = new Vector3(0.1f, 1, 1);
                alertText.text = "You were crushed by the intense gravity of the superheavy core.";
                klaxonCrush.Stop();
                klaxonEH.Stop();
                crushZoneText.enabled = false;
                EHText.enabled = false;
                move.dead = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            klaxonCrush.Stop();
            crushZoneText.enabled = false;
            hullSounds.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
