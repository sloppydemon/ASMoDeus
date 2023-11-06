using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

public class Move : MonoBehaviour
{
    public Rigidbody2D body;
    ParticleSystem thrustP;
    ParticleSystem thrustPL;
    ParticleSystem thrustPR;
    ParticleSystem thrustPF;
    ParticleSystem thrustPLS;
    ParticleSystem thrustPRS;
    float sumTorque;
    public float thrusterForceBoostFactor;
    public float brakeThrusterForce;
    public float thrusterForce;
    public float particlesPerThrust;
    public float thrustManeuverFactor;
    bool turnL;
    bool turnR;
    bool turning;
    float thrust;
    float thrustT;
    float particleRate;
    public float maxFuel;
    public float fuel;
    public float fuelUsePerThrust;
    float fuelUse;
    AudioSource thrustSndMain;
    AudioSource thrustSndL;
    AudioSource thrustSndR;
    public AudioClip thrustSndBoost;
    public AudioClip thrustSnd;
    public Slider fuelBar;
    public Image fuelBarFill;
    public Color fuelColorFull;
    public Color fuelColorEmpty;
    public GameObject mainThruster;
    public GameObject turnThrusterL;
    public GameObject turnThrusterR;
    public GameObject thrusterForward;
    public GameObject strafeThrusterL;
    public GameObject strafeThrusterR;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Handles.Label(body.position, $"Velocity:{body.velocity.magnitude}, Torque:{sumTorque}, Rotation:{body.angularVelocity} Turning:{turning}");
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        thrustSndMain = mainThruster.GetComponent<AudioSource>();
        thrustSndL = turnThrusterL.GetComponent<AudioSource>();
        thrustSndR = turnThrusterR.GetComponent<AudioSource>();
        thrustP = mainThruster.GetComponent<ParticleSystem>();
        thrustPL = turnThrusterL.GetComponent<ParticleSystem>();
        thrustPR = turnThrusterR.GetComponent<ParticleSystem>();
        thrustPF = thrusterForward.GetComponent<ParticleSystem>();
        thrustPLS = strafeThrusterL.GetComponent<ParticleSystem>();
        thrustPRS = strafeThrusterR.GetComponent<ParticleSystem>();
        var thrustE = thrustP.emission;
        var thrustLE = thrustPL.emission;
        var thrustRE = thrustPR.emission;
        var thrustFE = thrustPF.emission;
        var thrustLSE = thrustPLS.emission;
        var thrustRSE = thrustPRS.emission;
        thrustE.enabled = true;
        thrustLE.enabled = true;
        thrustRE.enabled = true;
        thrustFE.enabled = true;
        thrustLSE.enabled = true;
        thrustRSE.enabled = true;
        fuel = maxFuel;
        fuelBar.maxValue = maxFuel;
        fuelBar.value = fuel;

        thrustE.rate = 0;
        thrustLE.rate = 0;
        thrustRE.rate = 0;
        thrustFE.rate = 0;
        thrustLSE.rate = 0;
        thrustRSE.rate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var thrustE = thrustP.emission;
        var thrustLE = thrustPL.emission;
        var thrustRE = thrustPR.emission;
        var thrustFE = thrustPF.emission;
        var thrustLSE = thrustPLS.emission;
        var thrustRSE = thrustPRS.emission;
        var thrustS = thrustP.main.startSizeMultiplier;
        var thrustSMax = thrustP.main.startSize.constantMax;
        var thrustSLMin = thrustP.main.startSize.constantMin;
        var thrustSLMax = thrustP.main.startSize.constantMax;
        var thrustSRMin = thrustP.main.startSize.constantMin;
        var thrustSRMax = thrustP.main.startSize.constantMax;
        sumTorque += body.totalTorque;

        if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
        {
            thrust = 0.001f * thrusterForce * thrusterForceBoostFactor;
            thrustT = thrust * thrustManeuverFactor;
            particleRate = particlesPerThrust * thrusterForceBoostFactor;
            //thrustS = 1f + (0.1f * thrusterForceBoostFactor);
            //thrustSMax = 0.2f + (0.2f * thrusterForceBoostFactor);
            //thrustSLMin = 0.01f + (0.01f * thrusterForceBoostFactor);
            //thrustSLMax = 0.02f + (0.02f * thrusterForceBoostFactor);
            //thrustSRMin = 0.01f + (0.01f * thrusterForceBoostFactor);
            //thrustSRMax = 0.02f + (0.02f * thrusterForceBoostFactor);
            fuelUse = fuelUsePerThrust * thrusterForceBoostFactor;
            //thrustSndMain.clip = thrustSndBoost;
            thrustSndMain.volume = 0.75f;
            //thrustSndL.clip = thrustSndBoost;
            thrustSndL.volume = 0.5f;
            //thrustSndR.clip = thrustSndBoost;
            thrustSndR.volume = 0.5f;
        }
        else
        {
            thrust = 0.001f * thrusterForce;
            thrustT = thrust * thrustManeuverFactor;
            particleRate = particlesPerThrust;
            //thrustS = 1f;
            //thrustSMax = 2f;
            //thrustSLMin = 0.01f;
            //thrustSLMax = 0.02f;
            //thrustSRMin = 0.01f;
            //thrustSRMax = 0.02f;
            fuelUse = fuelUsePerThrust;
            //thrustSndMain.clip = thrustSnd;
            thrustSndMain.volume = 0.5f;
            //thrustSndL.clip = thrustSnd;
            thrustSndL.volume = 0.25f;
            //thrustSndR.clip = thrustSnd;
            thrustSndR.volume = 0.25f;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            body.AddForce(body.transform.up * thrust);
            fuel -= fuelUse;
            fuelBar.value = fuel;
            fuelBarFill.color = Color.Lerp(fuelColorEmpty, fuelColorFull, fuel/maxFuel);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (body.velocity.magnitude > 0f)
            {
                body.AddForce(body.velocity * -thrust * brakeThrusterForce);
                fuel -= fuelUse;
                fuelBar.value = fuel;
                fuelBarFill.color = Color.Lerp(fuelColorEmpty, fuelColorFull, fuel / maxFuel);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            thrustE.rate = particleRate;
            thrustSndMain.Play();

        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            thrustE.rate = 0;
            thrustSndMain.Stop();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            thrustFE.rate = particleRate;
            thrustRE.rate = particleRate;
            thrustLE.rate = particleRate;
            thrustLSE.rate = particleRate;
            thrustRSE.rate = particleRate;
            thrustSndMain.volume = 0.25f;
            thrustSndMain.Play();
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            thrustFE.rate = 0;
            thrustRE.rate = 0;
            thrustLE.rate = 0;
            thrustLSE.rate = 0;
            thrustRSE.rate = 0;
            thrustSndMain.volume = 0.5f;
            thrustSndMain.Stop();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                body.AddForce(body.transform.right * (-1 * thrustT) * 2);
                fuel -= fuelUse * thrustManeuverFactor * 2;
                fuelBar.value = fuel;
                fuelBarFill.color = Color.Lerp(fuelColorEmpty, fuelColorFull, fuel / maxFuel);
            }
            else
            {
                body.AddForceAtPosition(body.transform.right * (-1 * thrustT), turnThrusterL.transform.position);
                fuel -= fuelUse * thrustManeuverFactor;
                fuelBar.value = fuel;
                fuelBarFill.color = Color.Lerp(fuelColorEmpty, fuelColorFull, fuel / maxFuel);
                //body.AddTorque(1f * thrust);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                body.AddForce(body.transform.right * thrustT * 2);
                fuel -= fuelUse * thrustManeuverFactor * 2;
                fuelBar.value = fuel;
                fuelBarFill.color = Color.Lerp(fuelColorEmpty, fuelColorFull, fuel / maxFuel);
            }
            else
            {
                body.AddForceAtPosition(body.transform.right * thrustT, turnThrusterR.transform.position);
                fuel -= fuelUse * thrustManeuverFactor;
                fuelBar.value = fuel;
                fuelBarFill.color = Color.Lerp(fuelColorEmpty, fuelColorFull, fuel / maxFuel);
                //body.AddTorque(-1f * thrust);
            }
        }
        else
        {
            if (body.angularVelocity != 0f)
            {
                if (body.angularVelocity > 0f)
                {
                    body.AddForceAtPosition(body.transform.right * (0.001f * thrusterForce * 0.025f * body.angularVelocity), turnThrusterL.transform.position);
                    thrustLE.rate = 2f * body.angularVelocity;
                }
                else if (body.angularVelocity < 0f)
                {
                    body.AddForceAtPosition(body.transform.right * (-0.001f * thrusterForce * (-0.0251f * body.angularVelocity)), turnThrusterR.transform.position);
                    thrustRE.rate = -1f * 2f * body.angularVelocity;
                }
                //body.AddTorque(-1f * body.angularVelocity * 0.0005f * thrusterForce);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                thrustLE.rate = particleRate;
                thrustLSE.rate = particleRate;
                thrustSndL.Play();
            }
            else
            {
                turning = true;
                thrustLE.rate = particleRate;
                thrustLSE.rate = 0;
                thrustSndL.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            turning = false;
            thrustLE.rate = 0;
            thrustLSE.rate = 0;
            thrustPR.Emit(50);
            thrustSndL.Stop();
            //body.AddTorque(-1f * sumTorque);
            //body.AddForceAtPosition(body.transform.right * (0.1f * body.angularVelocity), turnThrusterR.transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                thrustRE.rate = particleRate;
                thrustRSE.rate = particleRate;
                thrustSndR.Play();
            }
            else
            {
                turning = true;
                thrustRE.rate = particleRate;
                thrustRSE.rate = 0;
                thrustSndR.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            turning = false;
            thrustRE.rate = 0;
            thrustRSE.rate = 0;
            thrustPL.Emit(50);
            thrustSndR.Stop();
            //body.AddTorque(-1f * sumTorque);
            //body.AddForceAtPosition(body.transform.right * (-0.1f * body.angularVelocity), turnThrusterL.transform.position);
        }

        
    }
}
