using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
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
    public AudioSource klaxonEH;
    public AudioSource klaxonCrush;
    public AudioClip clang;
    public AudioClip crystal;
    public float initialHitPoints;
    public float hitPoints;
    public float initialCrushResistance;
    public float crushResistance;
    public int crystals;
    public TextMeshProUGUI crystalsNumber;
    public TextMeshProUGUI coreArrow;
    public TextMeshProUGUI alertText;
    public string damageDeathText;
    public TextMeshProUGUI EHAlertText;
    public TextMeshProUGUI crushAlertText;
    public Slider proximityBar;
    public Slider proximityBarEH;
    public Slider proximityBarCrush;
    public Slider crushResistanceBar;
    public Slider hitPointsBar;
    public Image hitPointsFill;
    public Color hitPointsFull;
    public Color hitPointsEmpty;
    public float proximityStartAddend;
    public AudioClip explosionSound;
    GameObject[] destroyObjects;
    GameObject explosionGO;
    ParticleSystem explosion;
    public AudioSource hullSound;

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
        hitPoints = initialHitPoints;
        crushResistance = initialCrushResistance;
        //bRadius = (0.09f * coreMass) / ((0.01f * move.thrusterForce * move.thrusterForceBoostFactor * body.mass) * (0.01f * move.thrusterForce * move.thrusterForceBoostFactor));
        //tRadius = Mathf.Sqrt((coreMass) / (((0.01f * move.thrusterForce)/mass) * 0.09f));
        //bRadius = Mathf.Sqrt((coreMass) / (((0.01f * move.thrusterForce * move.thrusterForceBoostFactor)/mass) * 0.09f));
        tRadius = Mathf.Sqrt((0.09f * coreMass * mass) / ((0.001f * move.thrusterForce)*mass));
        bRadius = Mathf.Sqrt((0.09f * coreMass * mass) / ((0.001f * move.thrusterForce * move.thrusterForceBoostFactor)*mass));
        proximityBar.maxValue = tRadius + proximityStartAddend;
        proximityBarEH.maxValue = tRadius + proximityStartAddend;
        proximityBarCrush.maxValue = tRadius + proximityStartAddend;
        proximityBar.minValue = bRadius;
        proximityBarEH.minValue = bRadius;
        proximityBarCrush.minValue = bRadius;
        proximityBar.value = tRadius + proximityStartAddend;
        proximityBarEH.value = tRadius;
        proximityBarCrush.value = (bRadius + bRadius + tRadius) / 3;
        crushResistanceBar.maxValue = initialCrushResistance;
        crushResistanceBar.minValue = 0f;
        hitPointsBar.maxValue = initialHitPoints;
        hitPointsBar.minValue = 0f;
        EHThrust.radius = tRadius;
        EHBoost.radius = bRadius;
        crushZone.radius = (bRadius + bRadius + tRadius)/3;
        hitPointsBar.value = hitPoints;
        crushResistanceBar.value = crushResistance;
        explosionGO = GameObject.FindGameObjectWithTag("Explosion");
        explosion = explosionGO.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            shipSnd.pitch = 1 / body.velocity.magnitude;
            shipSnd.PlayOneShot(clang, body.velocity.magnitude/5);
            hitPoints -= 2 * body.velocity.magnitude;
            hitPointsBar.value = hitPoints;
            hitPointsFill.color = Color.Lerp(hitPointsEmpty, hitPointsFull, hitPoints / initialHitPoints);
            hullSound.pitch = 1 / body.velocity.magnitude;
            //shipSnd.Play();
        }

        if (collision.gameObject.tag == "Terrain")
        {
            shipSnd.pitch = 1 / body.velocity.magnitude;
            shipSnd.PlayOneShot(clang, body.velocity.magnitude / 5);
            hitPoints -= 1 * body.velocity.magnitude;
            hitPointsBar.value = hitPoints;
            hitPointsFill.color = Color.Lerp(hitPointsEmpty, hitPointsFull, hitPoints / initialHitPoints);
            hullSound.pitch = 1 / body.velocity.magnitude;
            //shipSnd.Play();
        }

        if (collision.gameObject.tag == "Crystal")
        {
            shipSnd.pitch = 1;
            shipSnd.PlayOneShot(crystal, 1);
            crystals += 1;
            Destroy(collision.gameObject);
            crystalsNumber.text = new string($"{crystals}");
        }

        if (hitPoints <= 0f)
        {
            klaxonCrush.Stop();
            klaxonEH.Stop();
            klaxonCrush.enabled = false;
            klaxonEH.enabled = false;
            EHAlertText.enabled = false;
            crushAlertText.enabled = false;
            alertText.text = damageDeathText;
            move.dead = true;
            destroyObjects = GameObject.FindGameObjectsWithTag("Hull");
            foreach (GameObject desObj in destroyObjects)
            {
                Destroy(desObj);
            }
            Destroy(body);
            explosion.Emit(40);
            shipSnd.PlayOneShot(explosionSound, 1);
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

        if (distance < tRadius + proximityStartAddend)
        {
            if (distance < tRadius)
            {
                if (distance < (bRadius + bRadius + tRadius) / 3)
                {
                    proximityBar.value = distance;
                    proximityBarEH.value = distance;
                    proximityBarCrush.value = distance;
                }
                else
                {
                    proximityBar.value = distance;
                    proximityBarEH.value = distance;
                }          
            }
            else
            {
                proximityBar.value = distance;
            }
        }
        
        
    }

}
