using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour , IDamageable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private Image healthImage;
    private float currentHealth;
    private Coroutine hideHealthBarCoroutine;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = maxHealth;
        SetHealthBarVisible(false);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Debug.Log($"Enemy took {amount} damage. HP: {currentHealth}/{maxHealth}");
        healthImage.fillAmount = (float)currentHealth / maxHealth;
        SetHealthBarVisible(true);

        if (hideHealthBarCoroutine != null)
            StopCoroutine(hideHealthBarCoroutine);

        hideHealthBarCoroutine = StartCoroutine(HideHealthBarAfterDelay(0.5f));

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        animator?.SetTrigger(AnimationUtilities.HURT);
    }

    private void Die()
    {
        Debug.Log("Enemy died.");

        animator?.SetTrigger(AnimationUtilities.DIE);
    }
    private void SetHealthBarVisible(bool visible)
    {
        if (healthImage != null && healthImage.transform.parent != null)
        {
            healthImage.transform.parent.gameObject.SetActive(visible);
        }
    }

    private IEnumerator HideHealthBarAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetHealthBarVisible(false);
    }
}
