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
    bool win;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        win = false;
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
            move.enabled = false;
            crystalsNum = grav.crystals;
            alertText.text = new string($"You escaped with {crystalsNum} crystals!");
            win = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (win == true)
        {
            cam.transform.position += new Vector3(0, 0, -0.01f);

            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}
