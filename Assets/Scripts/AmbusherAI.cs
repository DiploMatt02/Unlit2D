using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class AmbusherAI : MonoBehaviour
{

    [SerializeField] private float minWait = 2f; // Minimum wait time before ambushing
    [SerializeField] private float maxWait = 5f; // Maximum wait time before ambushing
    [SerializeField] private float warningTime = 0.8f; // Time the warning eyes are shown before lunging
    [SerializeField] private float maxLungeTime = 4f; // Maximum time the ambusher will lunge towards the player before giving up

    [SerializeField] private float lungeSpeed = 8f; //How fast the ambusher lunges towards the player

    [SerializeField] private Light2D warningEyes; // Reference to the warning eyes light component

    //Lead lunge to where the player is going to be
    [SerializeField] private float leadTime = 0.5f;
    [SerializeField] private float overshootDistance = 1.5f;

    //Stalk the player
    [SerializeField] private float stalkSpeed = 1.5f;
    [SerializeField] private float stalkMargin = 1.5f;

    private bool isLunging;

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

        if (isLunging)
        {
            speed = lungeSpeed;
        }
        else
        {
            speed = stalkSpeed;
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
            //Stalk
            isLunging = false;
            float stalkTime = Random.Range(minWait, maxWait);
            float stalkTimer = 0f;

            while (stalkTimer < stalkTime || stun.IsStunned())
            {
                float distance = Vector2.Distance(transform.position, player.position);
                float lightEdge = torch.GetLightRadius() + stalkMargin;

                agent.SetDestination(player.position);
                agent.isStopped = distance <= lightEdge;

                stalkTimer += Time.deltaTime;
                yield return null;
            }

            agent.isStopped = true;
            

            //Warn and watch player movement
            warningEyes.enabled = true;
            Vector3 startPosition = player.position;

            yield return new WaitForSeconds(warningTime);
            
            warningEyes.enabled = false;
            Vector3 endPosition = player.position;

            //Lead: aim where player is heading
            Vector3 playerVelocity = (endPosition - startPosition) / warningTime;
            Vector3 target = endPosition + playerVelocity * leadTime;
            
            //Overshoot: charge past the target
            Vector3 chargeDirection = (target - transform.position).normalized;
            target += chargeDirection * overshootDistance;

            //Keep target on walkable ground
            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                target = hit.position;
            }
            else
            {
                target = endPosition;
            }

            //Lunge
            isLunging = true;
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
