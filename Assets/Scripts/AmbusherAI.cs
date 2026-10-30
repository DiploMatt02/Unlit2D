using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class AmbusherAI : MonoBehaviour
{
    //Stalk before attacking
    [Header("Timing")]
    [SerializeField] private float minWait = 2f;
    [SerializeField] private float maxWait = 5f;
    [SerializeField] private float warningTime = 0.8f;
    [SerializeField] private float maxLungeTime = 4f;

    //Stalks outside light raidus
    [Header("Stalking")]
    [SerializeField] private float stalkSpeed = 1.5f;
    [SerializeField] private float stalkMargin = 1.5f;

    //Lunge speed while aiming ahead the player and charges past for a prediction hit
    [Header("Lunge")]
    [SerializeField] private float lungeSpeed = 8f;
    [SerializeField] private float leadTime = 0.5f;
    [SerializeField] private float overshootDistance = 1.5f;

    //Glowing eyes with sound before lunge attack
    [Header("Warning")]
    [SerializeField] private Light2D warningEyes;
    [SerializeField] private AudioClip warningSound;
    [SerializeField] private float warningVolume = 0.7f;

    //Live state and components found at runtime
    private bool isLunging;

    private NavMeshAgent agent;
    private Transform player;
    private TorchLight torch;

    private EnemyStun stun;


    void Start()
    {   
        //Grab components and turn off 3D rotation
        stun = GetComponent<EnemyStun>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //Find player
        player = GameObject.FindWithTag("Player").transform;
        torch = player.GetComponentInChildren<TorchLight>();

        //Start with warning eyes off and start the stalk/warn/lunge cycle
        warningEyes.enabled = false;
        StartCoroutine(AmbushLoop());
    }


    void Update()
    {
        //Pick speed for current state, stun halts everything
        float fuel = torch.GetFuelPercentage();
        float speed;

        if (isLunging)
        {
            speed = lungeSpeed;
        }
        else
        {
            speed = stalkSpeed;
        }

        if (stun.IsStunned())
        {
            speed = 0;
            agent.velocity = Vector3.zero;
            warningEyes.enabled = false;
        }
        agent.speed = speed;
    }
    
    //Cycle loop: stalk, warn, lunge
    IEnumerator AmbushLoop()
    {
        while (true)
        {
            //Stalk the player and wait for a random time at the light's edge
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

            //Freeze in place before the warning
            agent.isStopped = true;
            

            //Warning phase, eyes turn red and plays sound as it watches the player's movement
            warningEyes.enabled = true;
            SoundManager.Instance.Play(warningSound, warningVolume);
            Vector3 startPosition = player.position;

            yield return new WaitForSeconds(warningTime);
            
            warningEyes.enabled = false;
            Vector3 endPosition = player.position;


            //Lead where the player is heading, overshoot past in hopes of clipping player, keep on walkable ground
            Vector3 playerVelocity = (endPosition - startPosition) / warningTime;
            Vector3 target = endPosition + playerVelocity * leadTime;
            

            Vector3 chargeDirection = (target - transform.position).normalized;
            target += chargeDirection * overshootDistance;


            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                target = hit.position;
            }
            else
            {
                target = endPosition;
            }

            //Lunge until it arrives or give up after maxLungeTime
            isLunging = true;
            agent.isStopped = false;
            agent.SetDestination(target);

            float timer = 0f;
            while (timer < maxLungeTime && (agent.pathPending || agent.remainingDistance > 0.2f))
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
