using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float damageBlinkDuration = 0.1f;

    [SerializeField] private EndScreens endScreens;

    private int currentHealth;
    private bool isInvincible = false;
    private SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityBlink());
        }
    }

    IEnumerator InvincibilityBlink()
    {
        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            sprite.enabled = !sprite.enabled; // Toggle visibility
            yield return new WaitForSeconds(damageBlinkDuration);
            elapsedTime += damageBlinkDuration;
        }

        sprite.enabled = true; // Ensure the sprite is visible at the end
        isInvincible = false;
    }

    void Die()
    {
        endScreens.ShowLose();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
