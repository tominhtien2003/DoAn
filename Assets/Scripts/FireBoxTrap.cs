using UnityEngine;

public class FireBoxTrap : IStationaryTrap
{
    [Header("Firebox Trap Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fireObj;
    [SerializeField] private Vector2 hitBoxSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 hitBoxOffset = Vector2.zero;
    [SerializeField] private float lifeFireTime = 2f;

    private bool isTriggered = false;
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnCooldownComplete()
    {
        if (isTriggered && !isActivated)
        {
            TriggerTrap();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            if (isActivated)
            {
                DealDamageToPlayerInArea();
                return;
            }
            isTriggered = true;
            TriggerTrap();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            isTriggered = false;
        }
    }

    protected override void ExecuteTrap()
    {
        animator?.SetTrigger("Activate");
        fireObj?.SetActive(true);

        DealDamageToPlayerInArea();

        Invoke(nameof(ResetTrap), lifeFireTime);
    }
    protected override void ResetTrap()
    {
        base.ResetTrap();
        cooldownTimer = cooldownTime;
        animator?.SetBool(AnimationUtilities.ACTIVATE, false);
        fireObj.SetActive(false);
    }
    private void DealDamageToPlayerInArea()
    {
        Vector2 boxCenter = (Vector2)transform.position + hitBoxOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, hitBoxSize, 0f, playerMask);
        foreach (var hit in hits)
        {
            if (IsPlayer(hit.gameObject))
            {
                ApplyDamageTo(hit.transform.parent.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + hitBoxOffset, hitBoxSize);
    }
}
