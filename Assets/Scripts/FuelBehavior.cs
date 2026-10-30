using UnityEngine;

public class FuelBehavior : MonoBehaviour
{
    //Default seconds of fuel added and pickup sound
    [SerializeField] private float fuelAmount = 10f;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupVolume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Only the player's torch can pick it up; add fuel, play sound, remove pickup
        TorchLight torch = collision.GetComponent<TorchLight>();
        if (torch != null)
        {
            torch.AddFuel(fuelAmount);
            SoundManager.Instance.Play(pickupSound, pickupVolume);
            Destroy(gameObject);
        }
    }
}
