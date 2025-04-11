using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    [SerializeField] private KnightEnemy knightEnemy;

    public void OnAttackAnimationFinished()
    {
        knightEnemy?.OnAttackAnimationFinished();
    }
}
