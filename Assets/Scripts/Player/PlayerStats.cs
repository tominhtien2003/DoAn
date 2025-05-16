using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    private const string HEALTH_KEY = "PlayerMaxHealth";
    private const string DAMAGE_KEY = "PlayerDamage";
    private const string DEFAULT_HEALTH = "DefaultHealth";

    [SerializeField] private int defaultMaxHealth = 100;
    [SerializeField] private int defaultDamage = 10;

    public int MaxHealth { get; private set; }
    public int Damage { get; private set; }

    public event Action<int> OnHealthUpgraded;
    public event Action<int> OnDamageUpgraded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadStats()
    {
        if (!PlayerPrefs.HasKey(DEFAULT_HEALTH))
        {
            PlayerPrefs.SetInt(DEFAULT_HEALTH, defaultMaxHealth);
            PlayerPrefs.SetInt(HEALTH_KEY, defaultMaxHealth);
            PlayerPrefs.SetInt(DAMAGE_KEY, defaultDamage);
            PlayerPrefs.Save();
        }

        MaxHealth = PlayerPrefs.GetInt(HEALTH_KEY, defaultMaxHealth);
        Damage = PlayerPrefs.GetInt(DAMAGE_KEY, defaultDamage);
    }

    public void UpgradeHealth(int amount)
    {
        MaxHealth += amount;
        PlayerPrefs.SetInt(HEALTH_KEY, MaxHealth);
        PlayerPrefs.Save();
        OnHealthUpgraded?.Invoke(MaxHealth);
    }

    public void UpgradeDamage(int amount)
    {
        Damage += amount;
        PlayerPrefs.SetInt(DAMAGE_KEY, Damage);
        PlayerPrefs.Save();
        OnDamageUpgraded?.Invoke(Damage);
    }

    public void ResetStats()
    {
        OnHealthUpgraded?.Invoke(MaxHealth);
        OnDamageUpgraded?.Invoke(Damage);
    }
    public void RestoreDefaultStats()
    {
        MaxHealth = PlayerPrefs.GetInt(DEFAULT_HEALTH, defaultMaxHealth);
        Damage = defaultDamage;
        PlayerPrefs.SetInt(HEALTH_KEY, MaxHealth);
        PlayerPrefs.SetInt(DAMAGE_KEY, Damage);
        PlayerPrefs.Save();
        OnHealthUpgraded?.Invoke(MaxHealth);
        OnDamageUpgraded?.Invoke(Damage);
    }
} 