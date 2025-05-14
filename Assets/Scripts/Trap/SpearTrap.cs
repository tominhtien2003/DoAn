using UnityEngine;

public class SpearTrap : IStationaryTrap
{

    [Header("Spear Trap Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private Vector2 hitBoxSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 hitBoxOffset = Vector2.zero;

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
        animator?.SetBool(AnimationUtilities.ACTIVATE,true);

        DealDamageToPlayerInArea();

        Invoke(nameof(ResetTrap), .5f);
    }
    protected override void ResetTrap()
    {
        base.ResetTrap();
        cooldownTimer = cooldownTime;
        animator?.SetBool(AnimationUtilities.ACTIVATE, false);
    }
    private void DealDamageToPlayerInArea()
    {
        Vector2 boxCenter = (Vector2)transform.position + hitBoxOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, hitBoxSize, 0f, playerMask);
        foreach (var hit in hits)
        {
            if (IsPlayer(hit.gameObject))
            {
                ApplyDamageTo(hit.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + hitBoxOffset, hitBoxSize);
    }
}
