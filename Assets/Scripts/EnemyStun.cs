using UnityEngine;
using System.Collections;

public class EnemyStun : MonoBehaviour
{
    [SerializeField] private Color stunColor = Color.cyan;

    private bool isStunned;

    public bool IsStunned()
    {
        return isStunned;
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

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
