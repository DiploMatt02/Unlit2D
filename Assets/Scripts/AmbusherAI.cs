using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class AmbusherAI : MonoBehaviour
{
    [SerializeField] private float lightSpeedFullTorch = 1.5f; // Speed when the player has a full torch in light areas
    [SerializeField] private float lightSpeedDyingTorch = 3.5f; // Speed when the player has a dying torch in light areas
    [SerializeField] private float darkSpeedFullTorch = 5f; // Speed when the player has a full torch in dark areas
    [SerializeField] private float darkSpeedDyingTorch = 6f; // Speed when the player has a dying torch in dark areas

    [SerializeField] private float minWait = 2f; // Minimum wait time before ambushing
    [SerializeField] private float maxWait = 5f; // Maximum wait time before ambushing
    [SerializeField] private float warningTime = 0.8f; // Time the warning eyes are shown before lunging
    [SerializeField] private float maxLungeTime = 4f; // Maximum time the ambusher will lunge towards the player before giving up

    [SerializeField] private Light2D warningEyes; // Reference to the warning eyes light component

    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Transform player; // Reference to the player's transform
    private TorchLight torch; // Reference to the player's torch light component

    private EnemyStun stun; // Reference to the enemy stun component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        stun = GetComponent<EnemyStun>(); // Get the EnemyStun component attached to this GameObject
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to this GameObject
        agent.updateRotation = false; // Disable automatic rotation updates
        agent.updateUpAxis = false; // Disable automatic up axis updates

        player = GameObject.FindWithTag("Player").transform; // Find the player GameObject by tag and get its transform
        torch = player.GetComponentInChildren<TorchLight>(); // Get the TorchLight component from the player's children

        warningEyes.enabled = false; // Disable the warning eyes light at the start
        StartCoroutine(AmbushLoop()); // Start the ambush loop coroutine
    }

    // Update is called once per frame
    void Update()
    {
        float fuel = torch.GetFuelPercentage(); // Get the current fuel percentage of the torch
        float speed;

        if (torch.IsInLight(transform.position)) // Is ambusher in light area?
        {
            speed = Mathf.Lerp(lightSpeedDyingTorch, lightSpeedFullTorch, fuel); // Interpolate speed based on torch fuel in light areas
        } 
        else // Ambusher is in dark area
        {
            speed = Mathf.Lerp(darkSpeedDyingTorch, darkSpeedFullTorch, fuel); // Interpolate speed based on torch fuel in dark areas
        }
        if (stun.IsStunned()) // Check if the ambusher is stunned
        {
            speed = 0; // Set speed to 0 if stunned
            agent.velocity = Vector3.zero; // Stop the agent's movement
            warningEyes.enabled = false; //Disable Warning Eyes if stunned
        }
        agent.speed = speed; // Set the agent's speed based on the calculated value
    }

    IEnumerator AmbushLoop()
    {
        while (true)
        {
            //Lurk
            agent.isStopped = true; // Stop the agent from moving
            yield return new WaitForSeconds(Random.Range(minWait, maxWait)); // Wait for a random time between minWait and maxWait

            //Wait for Warning Eyes due to Flash
            while (stun.IsStunned())
            {
                yield return null;
            }

            //Warn
            Vector3 target = player.position; // Get the player's current position
            warningEyes.enabled = true; // Enable the warning eyes light
            yield return new WaitForSeconds(warningTime); // Wait for the warning time
            warningEyes.enabled = false; // Disable the warning eyes light

            //Lunge
            agent.isStopped = false; // Allow the agent to move
            agent.SetDestination(target); // Set the agent's destination to the player's position at the time of warning

            float timer = 0f; // Initialize a timer to track the lunge duration
            while (timer < maxLungeTime && (agent.pathPending || agent.remainingDistance > 0.2f)) // While the lunge time hasn't exceeded and the agent is still moving
            {
                timer += Time.deltaTime; // Increment the timer by the time elapsed since the last frame
                yield return null; // Wait for the next frame
            }
        }
    }
}
