using UnityEngine;
using UnityEngine.AI;

public class ChaserAI : MonoBehaviour
{
    [SerializeField] private float lightSpeedFullTorch = 1f;
    [SerializeField] private float lightSpeedDyingTorch = 3f;

    [SerializeField] private float darkSpeedFullTorch = 3f;
    [SerializeField] private float darkSpeedDyingTorch = 4.5f;

    private TorchLight torch;
    private NavMeshAgent agent;
    private Transform player;

    private EnemyStun stun;

    void Start()
    {
        stun = GetComponent<EnemyStun>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation= false;
        agent.updateUpAxis = false;

        player = GameObject.FindWithTag("Player").transform;
        torch = player.GetComponentInChildren<TorchLight>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed;

        if (torch.IsInLight(transform.position))
        {
            speed = Mathf.Lerp(lightSpeedDyingTorch, lightSpeedFullTorch, torch.GetFuelPercentage());
        }
        else
        {
            speed = Mathf.Lerp(darkSpeedDyingTorch, darkSpeedFullTorch, torch.GetFuelPercentage());
        }
        if (stun.IsStunned())
        {
            speed = 0;
            agent.velocity = Vector3.zero;
        }
        agent.speed = speed;
        
        agent.SetDestination(player.position);
    }
}

