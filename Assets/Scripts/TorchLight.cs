using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLight : MonoBehaviour
{
    [SerializeField] private Light2D torchLight; // Reference to the Light2D component attached to the torch
    [SerializeField] private float startingBurnTime = 20f; // Starting burn time in seconds
    [SerializeField] private float maxBurnTime = 20f; // Maximum burn time in seconds
    [SerializeField] private float fullRadius = 6f; // Full radius of the torch light
    [SerializeField] private float minRadius = 1f; // Minimum radius of the torch light
    [SerializeField] private float fullIntensity = 1.2f; // Full intensity of the torch light
    [SerializeField] private float minIntensity = 0.2f; // Minimum intensity of the torch light
    [SerializeField] private float smoothing = 4f; // Smoothing factor for how fast the light catches up to the fuel percentage
    [SerializeField] private float flickerAmount = 0.08f; //how much the light flickers based on the fuel percentage
    [SerializeField] private float flickerSpeed = 3f; //how fast the light flickers based on the fuel percentage
    private float burnTimeLeft; // Remaining burn time in seconds.
    private float displayFuel; // The fuel percentage that is currently being displayed\

    [SerializeField] private float flashFadeSpeed = 6f; // Speed at which the light fades in and out when flickering
    private float flashBoost; // Powerup boost for the light providing extra light for a short duration

    void Start()
    {
        burnTimeLeft = startingBurnTime; // Initialize burn time left to the starting burn time
    }

    void Update()
    {
        // Decrease the burn time left based on the time elapsed since the last frame
        burnTimeLeft -= Time.deltaTime;

        // Ensure burn time left doesn't go below zero
        burnTimeLeft = Mathf.Max(0f, burnTimeLeft);

        float fuel = burnTimeLeft / maxBurnTime; // Calculate the fuel percentage (0 to 1)

        displayFuel = Mathf.Lerp(displayFuel, fuel, Time.deltaTime * smoothing); // Smoothly interpolate the displayed fuel percentage

        float flicker = (Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) - 0.5f) * 2f *flickerAmount; // Calculate a flicker value based on Perlin noise
        
        torchLight.pointLightOuterRadius = Mathf.Lerp(minRadius, fullRadius, displayFuel); // Adjust the light radius based on displayed fuel percentage
        flashBoost = Mathf.MoveTowards(flashBoost, 0f, flashFadeSpeed * Time.deltaTime); // Gradually reduce the flash boost over time
        torchLight.intensity = Mathf.Lerp(minIntensity, fullIntensity, displayFuel) + flicker + flashBoost; // Adjust the light intensity based on displayed fuel percentage and add flicker
        torchLight.enabled = burnTimeLeft > 0f; // Enable or disable the light based on whether there is burn time left
    }

    public void AddFuel(float amount)
    {
        burnTimeLeft = Mathf.Min(burnTimeLeft + amount, maxBurnTime); // Ensure burn time left doesn't exceed the maximum burn time
    }
    public float GetFuelPercentage()
    {
        return displayFuel; // Return the current displayed fuel percentage
    }

    public bool IsInLight(Vector2 position)
    {
        if (burnTimeLeft <= 0f)
        {
            return false; // If the torch is out of fuel, it cannot provide light
        }

        float distance = Vector2.Distance(torchLight.transform.position, position); // Calculate the distance from the torch to the given position
        return distance <= torchLight.pointLightOuterRadius; // Return true if the position is within
    }

    public void TriggerFlash(float brightness)
    {
        flashBoost = brightness; // Set the flash boost to the specified brightness
    }

    public float GetLightRadius()
    {
        return torchLight.pointLightOuterRadius;
    }
}

