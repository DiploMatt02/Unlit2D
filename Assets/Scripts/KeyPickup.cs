using UnityEngine;

public class KeyPickup : MonoBehaviour
{
        private void OnTriggerEnter2D(Collider2D other)
    {
       KeyCollector collector = other.GetComponent<KeyCollector>();
        if (collector != null)
        {
            collector.CollectKey();
            Destroy(gameObject);
        }
    }
}
