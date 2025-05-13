using UnityEngine;

public class BrinerOfDeathAnimationRelay : MonoBehaviour
{
    [SerializeField] private BrinerofDeathBoss boss;
    private bool isHurting;
    public bool IsHurting
    {
        get { return isHurting; }
        set
        {
            isHurting = value;
            if (boss != null)
            {
                boss.IsHurting = isHurting;
            }
        }
    }
    public void StartHurt()
    {
        boss.GetStateMachine().ChangeState(null);
        boss.IsAttacking = false;
        IsHurting = true;
    }
    public void EndHurt()
    {
        IsHurting = false;
    }
    public void OnDie()
    {
        boss.enabled = false;
    }
    public void OnDestroyWhenDeath()
    {
        Destroy(boss.gameObject);
    }
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
