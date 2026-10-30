using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLight : MonoBehaviour
{
    //Light that the torch controls
    [Header("References")]
    [SerializeField] private Light2D torchLight;

    //Fuel size
    [Header("Fuel")]
    [SerializeField] private float startingBurnTime = 20f;
    [SerializeField] private float maxBurnTime = 20f;

    //Light size and brightness
    [Header("Light Range")]
    [SerializeField] private float fullRadius = 6f;
    [SerializeField] private float minRadius = 1f;
    [SerializeField] private float fullIntensity = 1.2f;
    [SerializeField] private float minIntensity = 0.2f;

    //flicker, flash burst, smooth raidus change for torch
    [Header("Effects")]
    [SerializeField] private float smoothing = 4f;
    [SerializeField] private float flickerAmount = 0.08f;
    [SerializeField] private float flickerSpeed = 3f;
    [SerializeField] private float flashFadeSpeed = 6f;
     
    //Live state
    private float burnTimeLeft;
    private float displayFuel; 
    private float flashBoost;
    private bool wasLit = true;


    //Torch sound loop
    [Header("Audio")]
    [SerializeField] private AudioSource burnSound;
    [SerializeField] private AudioClip extinguishSound;

    [SerializeField] private float extinguishVolume = 1f;

    void Start()
    {
        //Start the torch with its starting fuel
        burnTimeLeft = startingBurnTime;
    }

    void Update()
    {
        //Burn the fuel but never burn below zero
        burnTimeLeft -= Time.deltaTime;
        burnTimeLeft = Mathf.Max(0f, burnTimeLeft);

        //Convert fuel to 0-1 so it has smoother transition to empty
        float fuel = burnTimeLeft / maxBurnTime;
        displayFuel = Mathf.Lerp(displayFuel, fuel, Time.deltaTime * smoothing);

        //Flickering of the light and flash burst, follows the light whereever it goes
        float flicker = (Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) - 0.5f) * 2f *flickerAmount;
        
        torchLight.pointLightOuterRadius = Mathf.Lerp(minRadius, fullRadius, displayFuel);
        flashBoost = Mathf.MoveTowards(flashBoost, 0f, flashFadeSpeed * Time.deltaTime);
        torchLight.intensity = Mathf.Lerp(minIntensity, fullIntensity, displayFuel) + flicker + flashBoost;
        
        //Torch dies out
        torchLight.enabled = burnTimeLeft > 0f;

        if (burnTimeLeft > 0f && Time.timeScale > 0f)
        {
            burnSound.volume = Mathf.Lerp(0.1f,0.6f,displayFuel);
        }
        else
        {
            burnSound.volume = 0f;
        }

        bool isLit = burnTimeLeft > 0f;
        if (wasLit && !isLit)
        {
            SoundManager.Instance.Play(extinguishSound, extinguishVolume);
        }
        wasLit = isLit;
    }
    
    //Called by fuel pickup; caps at the max fuel size
    public void AddFuel(float amount)
    {
        burnTimeLeft = Mathf.Min(burnTimeLeft + amount, maxBurnTime);
    }

    //Hud, monsters, flash scripts all ask what the fuel is at
    public float GetFuelPercentage()
    {
        return displayFuel;
    }
    public bool IsLit()
    {
        return burnTimeLeft > 0f;
    }

    public bool IsInLight(Vector2 position)
    {
        if (burnTimeLeft <= 0f)
        {
            return false;
        }

        float distance = Vector2.Distance(torchLight.transform.position, position);
        return distance <= torchLight.pointLightOuterRadius;
    }

    //Called by flash powerup for a burst of light
    public void TriggerFlash(float brightness)
    {
        flashBoost = brightness;
    }

    //Ambusher asks for the light's size so it can stalk at the edge
    public float GetLightRadius()
    {
        return torchLight.pointLightOuterRadius;
    }
}

