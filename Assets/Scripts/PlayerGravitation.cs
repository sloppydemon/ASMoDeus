using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Gravitation : MonoBehaviour
{
    float distance;
    float distanceSquared;
    float mass;
    Rigidbody2D body;
    CoreMass well;
    Move move;
    Vector2 wellPos;
    Vector2 targetDirection;
    GameObject asteroid;
    GameObject EHThrustGC;
    GameObject EHBoostGC;
    GameObject crushZoneGC;
    CircleCollider2D EHThrust;
    CircleCollider2D EHBoost;
    CircleCollider2D crushZone;
    Rigidbody2D asteroidBody;
    float coreMass;
    float force;
    float bRadius;
    float tRadius;
    AudioSource shipSnd;
    public AudioClip clang;
    public float hitPoints;
    public float pressureResistance;
    int crystals;
    public TextMeshProUGUI coreArrow;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Handles.Label(wellPos, $"Force:{force}, Direction from Player:{targetDirection}");
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wellPos, bRadius);
        Handles.Label(wellPos + new Vector2(bRadius, bRadius), $"Boost Event Horizon: {bRadius}");
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wellPos, tRadius);
        Handles.Label(wellPos + new Vector2(tRadius, tRadius), $"Sub-Boost Event Horizon: {tRadius}");
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        shipSnd = GetComponent<AudioSource>();
        asteroid = GameObject.FindGameObjectWithTag("Asteroid");
        EHThrustGC = GameObject.FindGameObjectWithTag("EHThrust");
        EHBoostGC = GameObject.FindGameObjectWithTag("EHBoost");
        crushZoneGC = GameObject.FindGameObjectWithTag("CrushZone");
        EHThrust = EHThrustGC.GetComponent<CircleCollider2D>();
        EHBoost = EHBoostGC.GetComponent<CircleCollider2D>();
        crushZone = crushZoneGC.GetComponent<CircleCollider2D>();
        move = GetComponent<Move>();
        asteroidBody = asteroid.GetComponent<Rigidbody2D>();
        wellPos = asteroidBody.position;
        well = asteroid.GetComponent<CoreMass>();
        coreMass = well.mass;
        mass = body.mass;
        //bRadius = (0.09f * coreMass) / ((0.01f * move.thrusterForce * move.thrusterForceBoostFactor * body.mass) * (0.01f * move.thrusterForce * move.thrusterForceBoostFactor));
        //tRadius = Mathf.Sqrt((coreMass) / (((0.01f * move.thrusterForce)/mass) * 0.09f));
        //bRadius = Mathf.Sqrt((coreMass) / (((0.01f * move.thrusterForce * move.thrusterForceBoostFactor)/mass) * 0.09f));
        tRadius = Mathf.Sqrt((0.09f * coreMass * mass) / ((0.001f * move.thrusterForce)*mass));
        bRadius = Mathf.Sqrt((0.09f * coreMass * mass) / ((0.001f * move.thrusterForce * move.thrusterForceBoostFactor)*mass));
        EHThrust.radius = tRadius;
        EHBoost.radius = bRadius;
        crushZone.radius = (bRadius + bRadius + tRadius)/3;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            shipSnd.pitch = 1 - collision.rigidbody.velocity.magnitude;
            shipSnd.PlayOneShot(clang, body.velocity.magnitude/5);
            //shipSnd.Play();
        }

    }
    // Update is called once per frame
    void Update()
    {
        targetDirection = wellPos - body.position;
        distance = targetDirection.magnitude;
        distanceSquared = targetDirection.sqrMagnitude;
        force = ((coreMass + mass) / distanceSquared) * 0.09f;
        targetDirection = targetDirection.normalized;
        if (distance > 0f)
        {
            body.AddForce(targetDirection * force);
        }

        if (distance < tRadius)
        {

        }
    }

}