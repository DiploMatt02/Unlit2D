using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //Player scripts the HUD reads from
    [Header("Player Systems")]
    [SerializeField] private TorchLight torch;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private KeyCollector keys;
    [SerializeField] private FlashAbility flash;

    //Draining bar and its flame
    [Header("Torch Bar")]
    [SerializeField] private Image torchBarFill;
    [SerializeField] private Animator torchBarFlameAnimator;

    //Health icons
    [Header("Hearts")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Color fullHeartColor = Color.red;

    //Key icons
    [Header("Keys")]
    [SerializeField] private Image[] keyIcons;
    [SerializeField] private Color collectedKeyColor = new Color(1f, 0.85f, 0.2f);

    //Flash powerup icon
    [Header("Flash")]
    [SerializeField] private Image flashIcon;
    [SerializeField] private Color flashReadyColor = Color.white;

    //Gray tint for empty hearts, missing keys, and a used flash
    [Header("Shared")]
    [SerializeField] private Color emptyColor = new Color(0.25f, 0.25f, 0.25f);

    // Update is called once per frame
    void Update()
    {
        //Torch bar
        float fuel = torch.GetFuelPercentage();
        torchBarFill.fillAmount = fuel; // Smoothly drains and refills

        if (torchBarFlameAnimator != null)
        {
            torchBarFlameAnimator.SetBool("isLit", torch.IsLit()); // Flicker while lit, burn out at zero
        }

        //Hearts and keys
        UpdateIcons(hearts, health.GetCurrentHealth(), fullHeartColor);
        UpdateIcons(keyIcons, keys.GetKeysCollected(), collectedKeyColor);

        //Flash
        flashIcon.color = flash.HasFlash() ? flashReadyColor : emptyColor;
    }

    //Color the first icons up to the count, gray out the rest
    private void UpdateIcons(Image[] icons, int filledCount, Color filledColor)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (i < filledCount)
            {
                icons[i].color = filledColor;
            }
            else
            {
                icons[i].color = emptyColor;
            }
        }
    }
}
