using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
