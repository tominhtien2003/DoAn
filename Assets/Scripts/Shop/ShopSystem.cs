using UnityEngine;
using System;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance { get; private set; }

    [Header("Upgrade Costs")]
    [SerializeField] private int healthUpgradeCost = 5;
    [SerializeField] private int damageUpgradeCost = 10;

    [Header("Upgrade Values")]
    [SerializeField] private int healthUpgradeAmount = 1;
    [SerializeField] private int damageUpgradeAmount = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool TryUpgradeHealth(PlayerHealth playerHealth)
    {
        if (GameManager.Instance.CurrentCoins >= healthUpgradeCost)
        {
            if (GameManager.Instance.SpendCoins(healthUpgradeCost))
            {
                playerHealth.IncreaseMaxHealth(healthUpgradeAmount);
                return true;
            }
        }
        return false;
    }

    public bool TryUpgradeDamage(PlayerHealth playerHealth)
    {
        if (GameManager.Instance.CurrentCoins >= damageUpgradeCost)
        {
            if (GameManager.Instance.SpendCoins(damageUpgradeCost))
            {
                PlayerStats.Instance.UpgradeDamage(damageUpgradeAmount);
                return true;
            }
        }
        return false;
    }

    public int GetHealthUpgradeCost() => healthUpgradeCost;
    public int GetDamageUpgradeCost() => damageUpgradeCost;
} 