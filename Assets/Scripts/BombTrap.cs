using System.Collections;
using UnityEngine;

public class BombTrap : IStationaryTrap
{
    [Header("Bomb Trap Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gfx;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float radius = 2f;
    [SerializeField] private Vector2 offset;

    protected override void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            TriggerTrap();
        }
    }
    protected override void ExecuteTrap()
    {
        animator?.SetBool(AnimationUtilities.ACTIVATE, true);
        StartCoroutine(ExplodeAfterDelay(cooldownTime));
    }
    private void DealDamageToPlayerInArea()
    {
        Vector2 explosionCenter = (Vector2)transform.position + offset;

        Collider2D[] hits = Physics2D.OverlapCircleAll(explosionCenter, radius, playerMask);
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
        Vector2 explosionCenter = (Vector2)transform.position + offset;
        Gizmos.DrawWireSphere(explosionCenter, radius);
    }
    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DealDamageToPlayerInArea();

        if (gfx != null) gfx.SetActive(false);
        explosion.SetActive(true);
        Destroy(gameObject,.24f); 
    }
}
