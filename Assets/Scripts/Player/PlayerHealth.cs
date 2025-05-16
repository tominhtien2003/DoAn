using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    //[SerializeField] private Image healthImage;
    private float currentHealth;
    private float maxHealth;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
       
    }
    private void OnEnable()
    {
        maxHealth = PlayerStats.Instance.MaxHealth;
        currentHealth = maxHealth;
        if (InventoryManager.Instance != null && InventoryManager.Instance.healImage != null)
        {
            UpdateHealthUI();
        }
    }
    private void Start()
    {
        PlayerStats.Instance.OnHealthUpgraded += OnMaxHealthUpgraded;
    }

    private void OnDestroy()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthUpgraded -= OnMaxHealthUpgraded;
        }
    }

    private void OnMaxHealthUpgraded(int newMaxHealth)
    {
        float oldMaxHealth = maxHealth;
        maxHealth = newMaxHealth;
        
        float healthRatio = currentHealth / oldMaxHealth;
        currentHealth = maxHealth * healthRatio;
        
        UpdateHealthUI();
    }

    public void IncreaseMaxHealth(float amount)
    {
        PlayerStats.Instance.UpgradeHealth((int)amount);
    }   

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        animator.SetTrigger(AnimationUtilities.HURT);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.healImage != null)
        {
            InventoryManager.Instance.healImage.fillAmount = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player died.");
        AudioManager.Instance.PlaySound("Death");
        animator?.SetTrigger(AnimationUtilities.DIE);
    }
}
