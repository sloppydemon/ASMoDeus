using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public GameObject plyr;
    Vector3 camPos;
    // Start is called before the first frame update
    void Start()
    {
        //plyr = GameObject.FindGameObjectWithTag("Player");
        camPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        camPos = plyr.transform.position;
    }
}
