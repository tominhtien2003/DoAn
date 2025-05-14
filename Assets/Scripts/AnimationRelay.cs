using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    [SerializeField] private BaseEnemy enemy;
    private bool isHurting;
    public bool IsHurting
    {
        get { return isHurting; }
        set
        {
            isHurting = value;
            if (enemy != null)
            {
                enemy.IsHurting = isHurting;
            }
        }
    }

    public void OnAttackAnimationFinished()
    {
        enemy?.OnAttackAnimationFinished();  
    }

    public void CheckPlayer()
    {
        if (enemy != null && enemy.IsPlayerInAttackZone())
        {
            //Debug.Log("Player is in attack zone");
            enemy.ApplyDamageToPlayer();
        }
    }
    public void OnDie()
    {
        //Debug.Log("Die");
        enemy.enabled = false;
    }
    public void OnDestroyWhenDeath()
    {
        //Debug.Log("destroy");
        Destroy(enemy.gameObject);
    }
    public void StartHurt()
    {
        enemy.GetStateMachine().ExitCurrentState();
        enemy.IsAttacking = false;
        IsHurting = true;
    }
    public void EndHurt()
    {
        IsHurting = false;
    }
    public void OnStartAttack()
    {
        
    }
}
