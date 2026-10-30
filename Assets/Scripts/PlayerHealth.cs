using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //Health and the blinking safe time after being hit
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float damageBlinkDuration = 0.1f;

    //Shows the lose screen
    [SerializeField] private EndScreens endScreens;

    //Sound when hurt
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private float damageVolume = 1f;

    //Live state
    private int currentHealth;
    private bool isInvincible = false;
    private SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
    }

    //Called by enemies on contact; ignored while invincible
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        SoundManager.Instance.Play(damageSound, damageVolume);
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityBlink());
        }
    }

    //Blink and stay invincible for a short time after a hit
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

    //Out of health
    void Die()
    {
        endScreens.ShowLose();
    }

    //HUD asks for health
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
