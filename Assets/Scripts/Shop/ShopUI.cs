using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button healthUpgradeButton;
    [SerializeField] private Button damageUpgradeButton;
    [SerializeField] private TextMeshProUGUI txtBloodCurrent;
    [SerializeField] private TextMeshProUGUI txtDamageCurrent;

    public PlayerHealth playerHealth { get; set; }
    private ShopSystem shopSystem;
    private PlayerStats playerStats;

    private void Start()
    {
        shopSystem = ShopSystem.Instance;
        playerStats = PlayerStats.Instance;

        healthUpgradeButton.onClick.AddListener(OnHealthUpgradeClicked);
        damageUpgradeButton.onClick.AddListener(OnDamageUpgradeClicked);

        playerStats.OnHealthUpgraded += UpdateHealthUI;
        playerStats.OnDamageUpgraded += UpdateDamageUI;

        UpdateHealthUI(playerStats.MaxHealth);
        UpdateDamageUI(playerStats.Damage);
    }

    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthUpgraded -= UpdateHealthUI;
            playerStats.OnDamageUpgraded -= UpdateDamageUI;
        }
    }

    private void UpdateHealthUI(int newHealth)
    {
        if (txtBloodCurrent != null)
        {
            txtBloodCurrent.text = $"Blood : {newHealth}";
        }
    }

    private void UpdateDamageUI(int newDamage)
    {
        if (txtDamageCurrent != null)
        {
            txtDamageCurrent.text = $"Damage : {newDamage}";
        }
    }

    private void OnHealthUpgradeClicked()
    {
        if (shopSystem.TryUpgradeHealth(playerHealth))
        {
            // UI sẽ tự động cập nhật thông qua sự kiện OnHealthUpgraded
        }
    }

    private void OnDamageUpgradeClicked()
    {
        if (shopSystem.TryUpgradeDamage(playerHealth))
        {
            // UI sẽ tự động cập nhật thông qua sự kiện OnDamageUpgraded
        }
    }
} 