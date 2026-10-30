using UnityEngine;
using UnityEngine.AI;

public class ChaserAI : MonoBehaviour
{
    //Chaser's speed while in the light
    [SerializeField] private float lightSpeedFullTorch = 1f;
    [SerializeField] private float lightSpeedDyingTorch = 3f;

    //Chaser's speed in the dark
    [SerializeField] private float darkSpeedFullTorch = 3f;
    [SerializeField] private float darkSpeedDyingTorch = 4.5f;


    //Live state and components found at runtime
    private Vector3 lastPlayerPosition;

    private TorchLight torch;
    private NavMeshAgent agent;
    private Transform player;

    private EnemyStun stun;

    void Start()
    {
        //Grab components and turn off 3D rotation
        stun = GetComponent<EnemyStun>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation= false;
        agent.updateUpAxis = false;

        //Find player and their torch
        player = GameObject.FindWithTag("Player").transform;
        lastPlayerPosition = player.position;
        torch = player.GetComponentInChildren<TorchLight>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed;

        //Speed changes in dark and light dependent on the state of the torch
        if (torch.IsInLight(transform.position))
        {
            speed = Mathf.Lerp(lightSpeedDyingTorch, lightSpeedFullTorch, torch.GetFuelPercentage());
        }
        else
        {
            speed = Mathf.Lerp(darkSpeedDyingTorch, darkSpeedFullTorch, torch.GetFuelPercentage());
        }
        //Chaser got stunned
        if (stun.IsStunned())
        {
            speed = 0;
            agent.velocity = Vector3.zero;
        }
        agent.speed = speed;
        
        //Chase the player's current position
        Vector3 target = player.position;

        agent.SetDestination(target);
    }
}

