using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    //Sound when picked up
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupVolume = 1f;
        private void OnTriggerEnter2D(Collider2D other)
    {
       //Only the player can collect it; add key, play sound, remove pickup
       KeyCollector collector = other.GetComponent<KeyCollector>();
        if (collector != null)
        {
            collector.CollectKey();
            SoundManager.Instance.Play(pickupSound, pickupVolume);
            Destroy(gameObject);
        }
    }
}
