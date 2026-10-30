using UnityEngine;

public class FlashPickup : MonoBehaviour
{
    //Sound when picked up
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupVolume = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Only the player can pick it up; give the flash, play sound, remove pickup
        FlashAbility flash = other.GetComponent<FlashAbility>();

        if (flash != null)
        {
            flash.GiveFlash();
            SoundManager.Instance.Play(pickupSound, pickupVolume);
            Destroy(gameObject);
        }
    }
}

