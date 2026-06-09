using UnityEngine;
using System.Collections;

public class SciFiLightFlicker : MonoBehaviour
{
    // Properties
    public string waveFunction = "sin"; // possible values: sin, tri(angle), sqr(square), saw(tooth), inv(verted sawtooth), noise (random), lightning
    public float startValue = 0.0f; // start
    public float amplitude = 1.0f; // amplitude of the wave
    public float phase = 0.0f; // start point inside on wave cycle
    public float frequency = 0.5f; // cycle frequency per second

    public AudioClip lightningClip; // optional sound effect for lightning
    public float lightningProbabilityPerSecond = 0.02f; // chance per second to trigger a lightning flash
    public float lightningDuration = 0.1f; // duration of the lightning flash
    public float lightningIntensity = 4.0f; // how bright the flash is

    // Keep a copy of the original color
    private Color originalColor;
    private float lightningTimer = 0.0f;
    private AudioSource audioSource;

    // Store the original color
    void Start (){
        originalColor = GetComponent<Light>().color;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && lightningClip != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update (){
        Light light = GetComponent<Light>();

        if (waveFunction == "lightning")
        {
            UpdateLightning();
        }

        light.color = originalColor * (EvalWave());
    }

    void UpdateLightning()
    {
        if (lightningTimer > 0f)
        {
            lightningTimer -= Time.deltaTime;
            return;
        }

        if (Random.value < lightningProbabilityPerSecond * Time.deltaTime)
        {
            TriggerLightning();
        }
    }

    void TriggerLightning()
    {
        lightningTimer = lightningDuration;

        if (lightningClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(lightningClip);
        }
    }

    float EvalWave (){
        if (waveFunction == "lightning")
        {
            if (lightningTimer > 0f)
            {
                return lightningIntensity;
            }

            return startValue;
        }

        float x = (Time.time + phase)*frequency;
        float y;

        x = x - Mathf.Floor(x); // normalized value (0..1)

        if (waveFunction=="sin") {
            y = Mathf.Sin(x*2*Mathf.PI);
        }
        else if (waveFunction=="tri") {
            if (x < 0.5f)
                y = 4.0f * x - 1.0f;
            else
                y = -4.0f * x + 3.0f;  
        }    
        else if (waveFunction=="sqr") {
            if (x < 0.5f)
                y = 1.0f;
            else
                y = -1.0f;  
        }    
        else if (waveFunction=="saw") {
            y = x;
        }    
        else if (waveFunction=="inv") {
            y = 1.0f - x;
        }    
        else if (waveFunction=="noise") {
            y = 1 - (Random.value*2);
        }
        else {
            y = 1.0f;
        }        
        return (y*amplitude)+startValue;     
    }
}