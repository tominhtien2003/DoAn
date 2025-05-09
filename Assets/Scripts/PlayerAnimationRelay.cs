using UnityEngine;
using System.Collections;

public class PlayerAnimationRelay : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public void EndHurt()
    {
        if (playerController != null)
        {
            playerController.ResetStates();
        }
    }
    public void OnCheckEnemy()
    {
        if (playerController != null && playerController.IsEnemyInAttackZone())
        {
            playerController.ApplyDamageToEnemy();
        }
    }
    public void OnAttackFinish()
    {
        StartCoroutine(playerController.AttackCooldown());
        playerController.IsAttacking = false;
        playerController.UnlockMovement();
    }
}
