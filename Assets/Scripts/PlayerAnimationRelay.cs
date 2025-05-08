using UnityEngine;

public class PlayerAnimationRelay : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public void EndHurt()
    {
        playerController?.UnlockMovement();
        playerController?.playerAnimator.SetBool(AnimationUtilities.CAN_MOVE, true);
    }
    public void StartHurt()
    {
        playerController?.LockMovement();
        playerController?.playerAnimator.SetBool(AnimationUtilities.CAN_MOVE, false);
    }
    public void CheckEnemy()
    {
        if (playerController != null && playerController.IsEnemyInAttackZone())
        {
            //Debug.Log("Player is in attack zone");
            playerController.ApplyDamageToEnemy();
        }
    }
}
