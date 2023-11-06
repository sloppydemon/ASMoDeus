using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeEdge : MonoBehaviour
{
    public TextMeshProUGUI alertText;
    public AudioClip fanfare;
    AudioSource shipSnd;
    Move move;
    Gravitation grav;
    int crystalsNum;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            shipSnd = collision.gameObject.GetComponent<AudioSource>();
            shipSnd.PlayOneShot(fanfare);
            grav = collision.gameObject.GetComponent<Gravitation>();
            move = collision.gameObject.GetComponent<Move>();
            cam = collision.gameObject.GetComponent<Camera>();
            //move.enabled = false;
            move.win = true;
            crystalsNum = grav.crystals;
            alertText.text = new string($"You escaped with {crystalsNum} crystals!");
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
