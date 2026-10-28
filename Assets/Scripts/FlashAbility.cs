using UnityEngine;
using UnityEngine.InputSystem;

public class FlashAbility : MonoBehaviour
{
    [SerializeField] private InputActionReference flashActionReference;
    [SerializeField] private float stunDuration = 3f;
    [SerializeField] private float flashBrightness = 4f;

    private bool hasFlash;
    private TorchLight torch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        torch = GetComponentInChildren<TorchLight>();
    }

    void OnEnable()
    {
        flashActionReference.action.performed += OnFlashPerformed;
        flashActionReference.action.Enable();
    }

    void OnDisable()
    {
        flashActionReference.action.performed -= OnFlashPerformed;
        flashActionReference.action.Disable();
    }

    public void GiveFlash()
    {
        hasFlash = true;
    }

    public bool HasFlash()
    {
        return hasFlash;
    }

    private void OnFlashPerformed(InputAction.CallbackContext obj)
    {
        if (!hasFlash)
        {
            return;
        }

        hasFlash = false;
        torch.TriggerFlash(flashBrightness);

        EnemyStun[] enemies = FindObjectsOfType<EnemyStun>();
        foreach (EnemyStun enemy in enemies)
        {
            if (torch.IsInLight(enemy.transform.position))
            {
                enemy.Stun(stunDuration);
            }
        }
        
    }
}
