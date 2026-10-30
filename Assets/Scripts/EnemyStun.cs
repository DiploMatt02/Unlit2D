using UnityEngine;
using System.Collections;

public class EnemyStun : MonoBehaviour
{
    //Tint shown while stunned
    [SerializeField] private Color stunColor = Color.cyan;

    //Live state
    private bool isStunned;

    //AI scripts ask if they should stop moving
    public bool IsStunned()
    {
        return isStunned;
    }

    //Called by the flash powerup
    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    //Get stunned for a time
    IEnumerator StunRoutine(float duration)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color originalColor = sprite.color;

        isStunned = true;
        sprite.color = stunColor;

        yield return new WaitForSeconds(duration);

        sprite.color = originalColor;
        isStunned = false;
    }
}
