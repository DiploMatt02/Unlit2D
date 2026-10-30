using UnityEngine;
using UnityEngine.InputSystem;

public class FlashAbility : MonoBehaviour
{
    //F key input, how long enemies stay stunned, and how bright the flash bursts
    [SerializeField] private InputActionReference flashActionReference;
    [SerializeField] private float stunDuration = 3f;
    [SerializeField] private float flashBrightness = 4f;

    //Flash sound
    [SerializeField] private AudioClip flashSound;
    [SerializeField] private float flashVolume = 1f;

    //Live state
    private bool hasFlash;
    private TorchLight torch;
    void Start()
    {
        //Find the torch on the player
        torch = GetComponentInChildren<TorchLight>();
    }

    //Listen for the F key while active, stop listening when disabled
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

    //Called by the flash pickup
    public void GiveFlash()
    {
        hasFlash = true;
    }

    //HUD asks if the flash is ready
    public bool HasFlash()
    {
        return hasFlash;
    }

    private void OnFlashPerformed(InputAction.CallbackContext obj)
    {
        //Do nothing without a flash charge
        if (!hasFlash)
        {
            return;
        }

        //Use up the charge and burst the torch
        hasFlash = false;
        SoundManager.Instance.Play(flashSound, flashVolume);
        torch.TriggerFlash(flashBrightness);

        //Every enemy that is hit by the light from flash gets stunned
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
