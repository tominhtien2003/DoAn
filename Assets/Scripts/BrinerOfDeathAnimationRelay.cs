using UnityEngine;

public class BrinerOfDeathAnimationRelay : MonoBehaviour
{
    [SerializeField] private BrinerofDeathBoss boss;
    public void OnCheckPlayer()
    {
        if (boss.IsInMeleeAttackZone())
        {
            boss.ApplyDamageToPlayerByMeleeAttack();
        }
    }
    public void OnMeleeAttackEnd()
    {
        boss.IsAttacking = false;
    }
    public void OnMagicAttackEnd()
    {
    }
}
