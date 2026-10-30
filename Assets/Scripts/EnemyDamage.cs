using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    //Hearts removed per hit
    [SerializeField] private int damageAmount = 1;

    //If the enemy touches the player, damage taken by player
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
