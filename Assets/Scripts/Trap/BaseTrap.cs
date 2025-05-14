using UnityEngine;

public abstract class BaseTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected LayerMask playerMask;

    protected void ApplyDamageTo(GameObject obj)
    {
        if (obj.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(damage);
        }
    }

    protected bool IsPlayer(GameObject obj)
    {
        return obj != null && ((1 << obj.layer) & playerMask) != 0;
    }
}
