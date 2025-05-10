using UnityEngine;

public class DarkOrb : MonoBehaviour
{
    [SerializeField]
    private float damage = 15f;
    [SerializeField] LayerMask playerLayer;
    private bool isInAttackZone = false;

    private IDamageable playerHealth;
    public void OnEndAttack()
    {
        Destroy(gameObject);
    }
    public void OnCheckPlayer()
    {
        if (isInAttackZone)
        {
            playerHealth?.TakeDamage(damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            isInAttackZone = true;
            playerHealth = collision.GetComponent<IDamageable>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            isInAttackZone = false;
            playerHealth = null;
        }
    }
}