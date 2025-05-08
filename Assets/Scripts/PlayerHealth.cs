using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour , IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Animator animator;
    [SerializeField] private Image healthImage;
    private float currentHealth;
    public bool IsDead => currentHealth <= 0;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthImage.fillAmount = (float)currentHealth / maxHealth;

        //Debug.Log($"Player took {amount} damage. HP: {currentHealth}/{maxHealth}");

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
    }

    private void Die()
    {
        Debug.Log("Player died.");
        animator?.SetTrigger(AnimationUtilities.DIE);
    }
}
