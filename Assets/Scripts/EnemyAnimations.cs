using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimations : MonoBehaviour
{
    //Components on this monster
    private NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer sprite;

    void Start()
    {
        //Grab components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        //Is there any movement?
        Vector3 velocity = agent.velocity;
        bool isMoving = velocity.sqrMagnitude > 0.01f;

        //Idle if not moving, Walk if moving
        animator.SetBool("isMoving", isMoving);
        
        //Flip the sprite for left and right direction
        if (velocity.x < -0.05f)
        {
            sprite.flipX = true;
        }

        if (velocity.x > 0.05f)
        {
            sprite.flipX = false;
        }
    }
}
