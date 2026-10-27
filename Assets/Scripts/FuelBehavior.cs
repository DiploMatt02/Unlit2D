using UnityEngine;

public class FuelBehavior : MonoBehaviour
{
    [SerializeField] private float fuelAmount = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TorchLight torch = collision.GetComponent<TorchLight>();
        if (torch != null)
        {
            torch.AddFuel(fuelAmount);
            Destroy(gameObject);
        }
    }
}
