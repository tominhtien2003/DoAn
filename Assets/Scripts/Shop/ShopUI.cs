using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button healthUpgradeButton;
    [SerializeField] private Button damageUpgradeButton;

    public PlayerHealth playerHealth { get; set; }
    private ShopSystem shopSystem;

    private void Start()
    {
        shopSystem = ShopSystem.Instance;

        healthUpgradeButton.onClick.AddListener(OnHealthUpgradeClicked);
        damageUpgradeButton.onClick.AddListener(OnDamageUpgradeClicked);

    }
    private void OnHealthUpgradeClicked()
    {
        shopSystem.TryUpgradeHealth(playerHealth);
    }

    private void OnDamageUpgradeClicked()
    {
        shopSystem.TryUpgradeDamage(playerHealth);
    }
} 