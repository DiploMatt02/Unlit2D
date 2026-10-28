using UnityEngine;

public class FlashPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        FlashAbility flash = other.GetComponent<FlashAbility>();

        if (flash != null)
        {
            flash.GiveFlash();
            Destroy(gameObject);
        }
    }
}
